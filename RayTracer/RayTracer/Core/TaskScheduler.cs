using System.Collections.Generic;
using System.Threading;
using System;

using TaskGroupHandle = System.Int32;

namespace RayTracer
{
    public delegate void WorkerFunc(object context);

    public class Task
    {
        public Task(WorkerFunc workerFunc, object context)
        {
            worker = workerFunc;
            ctx = context;
        }

        public void Run()
        {
            worker(ctx);
        }

        private WorkerFunc worker;
        private object ctx;
    }

    public class TaskGroup
    {
        public TaskGroup()
        {
            tasks = new List<Task>();
            taskLock = new object();
        }

        public void Initialize()
        {
            taskHead = 0;
            reference = 0;
        }

        public Task GetNextTask()
        {
            lock(taskLock)
            {
                if (taskHead < tasks.Count)
                {
                    return tasks[taskHead++];
                }
            }

            return null;
        }

        public void AddTask(Task task)
        {
            lock(taskLock)
            {
                tasks.Add(task);
                AddRef();
            }
        }

        public bool HasTask()
        {
            return free == 0 && reference > 0;
        }

        public bool TryToUse()
        {
            return Interlocked.CompareExchange(ref free, 0, 1) == 1;
        }

        public void AddRef()
        {
            Interlocked.Increment(ref reference);
        }

        public void ReleaseRef()
        {
            Interlocked.Decrement(ref reference);
        }

        private List<Task> tasks;
        private int taskHead;
        private int reference;
        private int free = 1;
        private object taskLock;

        public int Free { get => free; set => free = value; }
        public int Reference { get => reference; }
    }

    public class TaskScheduler : IDisposable
    {
        public TaskScheduler()
        {
            threads = new List<Thread>(maxWorker);
            workerEvents = new List<ManualResetEventSlim>(maxWorker);

            for (int i = 0; i < maxWorker; ++i)
            {
                workerEvents.Add(new ManualResetEventSlim(false));

                Thread t = new Thread(new ParameterizedThreadStart(WorkerMain));
                t.Start(i);
                threads.Add(t);
            }

            taskGroups = new List<TaskGroup>(maxGroup);

            for (int i = 0; i < maxGroup; ++i)
            {
                taskGroups.Add(new TaskGroup());
            }
        }

        ~TaskScheduler()
        {
            Dispose(false);
        }

        private void WorkerMain(object threadID)
        {
            int id = (int)threadID;

            while (!isShutdown)
            {
                workerEvents[id].Wait();
                workerEvents[id].Reset();

                while (!isShutdown)
                {
                    Task task = null;
                    TaskGroup group = null;
                    for (int i = 0; i < maxGroup; ++i)
                    {
                        group = taskGroups[i];
                        if (group.HasTask())
                        {
                            task = group.GetNextTask();
                            if (task != null)
                            {
                                break;
                            }
                        }
                    }

                    if (task != null)
                    {
                        task.Run();
                        group.ReleaseRef();
                    }
                    else
                    {
                        group.Free = 1;
                        break;
                    }
                }
            }
        }

        public TaskGroupHandle GetFreeTaskGroup()
        {
            for (int i = 0; i < maxGroup; ++i)
            {
                var group = taskGroups[i];
                if (group.TryToUse())
                {
                    group.Initialize();
                    return i;
                }
            }

            return -1;
        }

        public void AddTask(TaskGroupHandle handle, Task task)
        {
            System.Diagnostics.Debug.Assert(handle < maxGroup);

            taskGroups[handle].AddTask(task);
        }

        public void WaitAll()
        {
            for (int i = 0; i < maxGroup; ++i)
            {
                Wait(i);
            }
        }

        public void Wait(TaskGroupHandle handle)
        {
            System.Diagnostics.Debug.Assert(handle < maxGroup);

            TaskGroup group = taskGroups[handle];
            while (true)
            {
                Task task = null;
                if (group.HasTask())
                {
                    task = group.GetNextTask();
                }

                if (task != null)
                {
                    task.Run();
                    group.ReleaseRef();
                }
                else
                {
                    while(true)
                    {
                        if (group.Reference != 0)
                        {
                            Thread.Yield();
                        }
                        else
                        {
                            group.Free = 1;
                            return;
                        }
                    }
                }
            }
        }

        public void Run()
        {
            for (int i = 0; i < maxWorker; ++i)
            {
                workerEvents[i].Set();
            }
        }

        protected virtual void Dispose(bool _)
        {
            if (disposed)
            {
                return;
            }

            isShutdown = true;
            Run();
            for (int i = 0; i < maxWorker; ++i)
            {
                threads[i].Join();
            }

            disposed = false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private List<TaskGroup> taskGroups;
        private List<Thread> threads;
        private List<ManualResetEventSlim> workerEvents;
        private volatile bool isShutdown;
        private bool disposed;
        private readonly int maxWorker = System.Environment.ProcessorCount;
        private readonly int maxGroup = System.Environment.ProcessorCount;
    }
}
