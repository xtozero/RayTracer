using Newtonsoft.Json.Linq;

namespace RayTracer
{
    public static class JsonWorldReader
    {
        public static World BuildWolrd(string filePath)
        {
            JObject worldAsset = new JObject();

            World w = new World();
            return w;
        }
    }
}
