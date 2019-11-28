using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RayTracer
{
    public class Scene
    {
        public void Capture(string imageName)
        {
            Canvas canvas = Canvas.Render(Camera, World, LightingModel);

            using (StreamWriter sw = new StreamWriter(imageName))
            {
                sw.Write(canvas.ToPPM());
            }
        }

        public Scene()
        {
            World = new World();
            LightingModel = new PhongReflection();
        }

        public Camera Camera { get; set; }
        public World World { get; set; }
        public LightingModel LightingModel { get; set; }
    }

    public class JsonScene : Scene
    {
        public JsonScene(string sceneName)
        {
            using (StreamReader sr = File.OpenText(sceneName))
            {
                using (JsonTextReader textReader = new JsonTextReader(sr))
                {
                    var jScene = JToken.ReadFrom(textReader);
                    var defineTable = new Dictionary<string, object>();

                    var camera = jScene.Value<JObject>("Camera");
                    Debug.Assert(camera != null);
                    Camera = CreateCamera(camera);

                    var lights = jScene.Value<JArray>("Lights");
                    if (lights != null)
                    {
                        foreach (var light in lights)
                        {
                            var property = GetKeyValuePair(light);
                            if (property.Key == "PointLight")
                            {
                                World.Lights.Add(CreatePointLight(property.Value));
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }

                    var define = jScene.Value<JObject>("Define");
                    if (define != null)
                    {
                        foreach(var item in define)
                        {
                            var property = GetKeyValuePair(item.Value);
                            defineTable.Add(item.Key, CreateEntity(property, defineTable));
                        }
                    }

                    var scene = jScene.Value<JArray>("Scene");
                    Debug.Assert(scene != null);
                    foreach (var entity in scene)
                    {
                        var property = GetKeyValuePair(entity);
                        World.Shapes.Add(CreateShape(property, defineTable));
                    }
                }
            }
        }

        private static Camera CreateCamera(JToken jToken)
        {
            int width = (int)jToken["Width"];
            int height = (int)jToken["Height"];
            float fov = (float)jToken["Fov"];

            var camera = new Camera(width, height, fov);

            var from = jToken["From"];
            var to = jToken["To"];
            var up = jToken["Up"];

            camera.Transform = Transformation.LookAt(ReadPoint(from), ReadPoint(to), ReadVector(up));

            return camera;
        }

        private static PointLight CreatePointLight(JToken jToken)
        {
            var at = jToken["At"];
            var intensity = jToken["Intensity"];

            return new PointLight(ReadPoint(at), ReadColor(intensity));
        }

        private static Material CreateMaterial(JToken jToken)
        {
            var material = new Material();

            var color = jToken["Color"];
            if (color != null)
            {
                material.Color = ReadColor(color);
            }

            var ambient = jToken["Ambient"];
            if (ambient != null)
            {
                material.Ambient = (float)ambient;
            }

            var diffuse = jToken["Diffuse"];
            if (diffuse != null)
            {
                material.Diffuse = (float)diffuse;
            }

            var specular = jToken["Specular"];
            if (specular != null)
            {
                material.Specular = (float)specular;
            }

            var shininess = jToken["Shininess"];
            if (shininess != null)
            {
                material.Shininess = (float)shininess;
            }

            var pattern = jToken["Pattern"];
            if (pattern != null)
            {
                material.Pattern = CreatePattern(pattern);
            }

            var reflective = jToken["Reflective"];
            if (reflective != null)
            {
                material.Reflective = (float)reflective;
            }

            var transparency = jToken["Transparency"];
            if (transparency != null)
            {
                material.Transparency = (float)transparency;
            }

            var refractiveIndex = jToken["RefractiveIndex"];
            if (refractiveIndex != null)
            {
                material.RefractiveIndex = (float)refractiveIndex;
            }

            return material;
        }

        private static Pattern CreatePattern(JToken jToken)
        {
            Pattern pattern = null;

            var property = GetKeyValuePair(jToken);
            switch (property.Key)
            {
                case "Stripes":
                    {
                        var colors = property.Value["Colors"];
                        pattern = new StripePattern(ReadColor(colors[0]), ReadColor(colors[1]));
                    }
                    break;
                case "Checkers":
                    {
                        var colors = property.Value["Colors"];
                        pattern = new CheckersPattern(ReadColor(colors[0]), ReadColor(colors[1]));
                    }
                    break;
                case "Ring":
                    {
                        var colors = property.Value["Colors"];
                        pattern = new RingPattern(ReadColor(colors[0]), ReadColor(colors[1]));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            var transform = property.Value["Transform"];
            if (transform != null)
            {
                pattern.Transform = CreateTransform(transform);
            }

            return pattern;
        }

        private static Type CreateShape<Type>(JToken jToken, Dictionary<string, object> defineTable) where Type : Shape, new()
        {
            Type shape = new Type();

            var transform = jToken["Transform"];
            if (transform != null)
            {
                shape.Transform = CreateTransform(transform);
            }

            var material = jToken["Material"];
            if (material != null)
            {
                if (material.Type == JTokenType.String)
                {
                    shape.Material = defineTable[(string)material] as Material;
                }
                else
                {
                    shape.Material = CreateMaterial(material);
                }
            }

            return shape;
        }

        private static Type CreateCylinderBasedShape<Type>(JToken jToken, Dictionary<string, object> defineTable) where Type :Cylinder, new()
        {
            Type shape = CreateShape<Type>(jToken, defineTable);

            var min = jToken["Min"];
            if (min != null)
            {
                shape.Minimum = (float)min;
            }

            var max = jToken["Max"];
            if (max != null)
            {
                shape.Maximum = (float)max;
            }

            var closed = jToken["Closed"];
            if (closed != null)
            {
                shape.Closed = (bool)closed;
            }

            return shape;
        }

        private static Shape CreateGroup(JToken jToken, Dictionary<string, object> defineTable)
        {
            var group = new Group();

            var children = jToken.Value<JArray>("Children");
            Debug.Assert(children != null);

            foreach(var child in children)
            {
                var property = GetKeyValuePair(child);
                group.AddChild(CreateShape(property, defineTable));
            }

            var transform = jToken["Transform"];
            if (transform != null)
            {
                group.Transform = CreateTransform(transform);
            }

            return group;
        }

        private static Shape CreateFromDefine(KeyValuePair<string, JToken> property, Dictionary<string, object> defineTable)
        {
            if (defineTable.ContainsKey(property.Key) == false)
            {
                throw new KeyNotFoundException($"{property.Key} is invalid key");
            }

            Shape src = defineTable[property.Key] as Shape;
            if (src == null)
            {
                throw new InvalidCastException($"{property.Key} is not Shape");
            }

            Shape clone = src.Clone() as Shape;
            if (clone == null)
            {
                throw new InvalidCastException(src.ToString() + " is not Shape");
            }

            var transform = property.Value["Transform"];
            if (transform != null)
            {
                clone.Transform = CreateTransform(transform);
            }

            var material = property.Value["Material"];
            if (material != null)
            {
                if (material.Type == JTokenType.String)
                {
                    clone.Material = defineTable[(string)material] as Material;
                }
                else
                {
                    clone.Material = CreateMaterial(material);
                }
            }

            return clone;
        }

        private static object CreateEntity(KeyValuePair<string, JToken> property, Dictionary<string, object> defineTable)
        {
            if (property.Key == "Material")
            {
                return CreateMaterial(property.Value);
            }
            else
            {
                return CreateShape(property, defineTable);
            }
        }

        private static Shape CreateShape(KeyValuePair<string, JToken> property, Dictionary<string, object> defineTable)
        {
            switch (property.Key)
            {
                case "Plane":
                    return CreateShape<Plane>(property.Value, defineTable);
                case "Sphere":
                    return CreateShape<Sphere>(property.Value, defineTable);
                case "Cube":
                    return CreateShape<Cube>(property.Value, defineTable);
                case "Cylinder":
                    return CreateCylinderBasedShape<Cylinder>(property.Value, defineTable);
                case "Cone":
                    return CreateCylinderBasedShape<Cone>(property.Value, defineTable);
                case "Group":
                    return CreateGroup(property.Value, defineTable);
                default:
                    return CreateFromDefine(property, defineTable);
            }
        }

        private static Matrix CreateTransform(JToken jToken)
        {
            var result = Matrix.Identity();

            foreach (var element in jToken)
            {
                var transform = GetKeyValuePair(element);
                switch (transform.Key)
                {
                    case "Scale":
                        {
                            var value = transform.Value;
                            var scale = ReadPoint(value);
                            result = Transformation.Scaling(scale.X, scale.Y, scale.Z) * result;
                        }
                        break;
                    case "RotateX":
                        {
                            result = Transformation.RotationX((float)transform.Value) * result;
                        }
                        break;
                    case "RotateY":
                        {
                            result = Transformation.RotationY((float)transform.Value) * result;
                        }
                        break;
                    case "RotateZ":
                        {
                            result = Transformation.RotationZ((float)transform.Value) * result;
                        }
                        break;
                    case "Translation":
                        {
                            var value = transform.Value;
                            var translation = ReadPoint(value);
                            result = Transformation.Translation(translation.X, translation.Y, translation.Z) * result;
                        }
                        break;
                    default:
                        throw new NotImplementedException($"invalid syntax {transform.Key} used");
                }
            }

            return result;
        }

        private static Tuple ReadPoint(JToken jToken)
        {
            return Tuple.Point((float)jToken[0], (float)jToken[1], (float)jToken[2]);
        }

        private static Tuple ReadColor(JToken jToken)
        {
            return Tuple.Color((float)jToken[0], (float)jToken[1], (float)jToken[2]);
        }

        private static Tuple ReadVector(JToken jToken)
        {
            return Tuple.Vector((float)jToken[0], (float)jToken[1], (float)jToken[2]);
        }

        private static KeyValuePair<string, JToken> GetKeyValuePair(JToken jToken)
        {
            var jObject = jToken.ToObject<JObject>();
            var keyValueEnum = jObject.GetEnumerator();
            if (keyValueEnum?.MoveNext() ?? false)
            {
                return keyValueEnum.Current;
            }
            else
            {
                throw new InvalidOperationException(@"can't move next");
            }
        }
    }
}
