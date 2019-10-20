using System;
using System.Collections.Generic;
using System.IO;
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
                JArray jScene = (JArray)JToken.ReadFrom(new JsonTextReader(sr));
                var symbolTable = new Dictionary<string, Object>();

                foreach (var element in jScene)
                {
                    string key = (string)element["Class"];
                    if (key == "Camera")
                    {
                        ParseCamera(element);
                    }
                    else if (key == "PointLight")
                    {
                        ParsePointLight(element);
                    }
                    else if (key == "Material")
                    {
                        ParseMaterial(element, symbolTable);
                    }
                    else if (key == "Shape")
                    {
                        ParseShape(element, symbolTable);
                    }
                }
            }
        }

        private void ParseCamera(JToken token)
        {
            int width = (int)token["Width"];
            int height = (int)token["Height"];
            float fov = (float)token["Fov"];

            Camera = new Camera(width, height, fov);

            var from = token["From"];
            var to = token["To"];
            var up = token["Up"];

            Camera.Transform = Transformation.LookAt(Tuple.Point((float)from[0], (float)from[1], (float)from[2]), 
                                                    Tuple.Point((float)to[0], (float)to[1], (float)to[2]), 
                                                    Tuple.Vector((float)up[0], (float)up[1], (float)up[2]));
        }

        private void ParsePointLight(JToken token)
        {
            var at = token["At"];
            var intensity = token["Intensity"];

            World.Light = new PointLight(Tuple.Point((float)at[0], (float)at[1], (float)at[2]),
                                        Tuple.Color((float)intensity[0], (float)intensity[1], (float)intensity[2]));
        }

        private Material ParseMaterial(JToken token, Dictionary<string, Object> symbolTable)
        {
            Material m = new Material();

            var color = token["Color"];
            if (color != null)
            {
                m.Color = Tuple.Color((float)color[0], (float)color[1], (float)color[2]);
            }

            var ambient = token["Ambient"];
            if (ambient != null)
            {
                m.Ambient = (float)ambient;
            }

            var diffuse = token["Diffuse"];
            if (diffuse != null)
            {
                m.Diffuse = (float)diffuse;
            }

            var specular = token["Specular"];
            if (specular != null)
            {
                m.Specular = (float)specular;
            }

            var shininess = token["Shininess"];
            if (shininess != null)
            {
                m.Shininess = (float)shininess;
            }

            var pattern = token["Pattern"];
            if (pattern != null)
            {
                m.Pattern = ParsePattern(pattern);
            }

            var reflective = token["Reflective"];
            if (reflective != null)
            {
                m.Reflective = (float)reflective;
            }

            var transparency = token["Transparency"];
            if (transparency != null)
            {
                m.Transparency = (float)transparency;
            }

            var refractiveIndex = token["RefractiveIndex"];
            if (refractiveIndex != null)
            {
                m.RefractiveIndex = (float)refractiveIndex;
            }

            var name = token["Name"];
            if (name != null)
            {
                symbolTable.Add((string)name, m);
            }

            return m;
        }

        private Pattern ParsePattern(JToken token)
        {
            Pattern pattern = null;

            string type = (string)token["Type"];
            if (type == "Stripes")
            {
                var colors = token["Colors"];
                pattern = new StripePattern(Tuple.Color((float)colors[0][0], (float)colors[0][1], (float)colors[0][2]),
                                            Tuple.Color((float)colors[1][0], (float)colors[1][1], (float)colors[1][2]));
            }
            else if (type == "Checkers")
            {
                var colors = token["Colors"];
                pattern = new CheckersPattern(Tuple.Color((float)colors[0][0], (float)colors[0][1], (float)colors[0][2]),
                                            Tuple.Color((float)colors[1][0], (float)colors[1][1], (float)colors[1][2]));
            }

            var transform = token["Transform"];
            if (transform != null)
            {
                pattern.Transform = ParseTransform(transform);
            }

            return pattern;
        }

        private void ParseShape(JToken token, Dictionary<string, Object> symbolTable)
        {
            Shape shape = null;

            string type = (string)token["Type"];
            if (type == "Plane")
            {
                shape = new Plane();
            }
            else if (type == "Sphere")
            {
                shape = new Sphere();
            }

            var transform = token["Transform"];
            if (transform != null)
            {
                shape.Transform = ParseTransform(transform);
            }

            var material = token["Material"];
            if (material != null)
            {
                if (material.Type == JTokenType.String)
                {
                    shape.Material = (Material)symbolTable[(string)material];
                }
                else
                {
                    shape.Material = ParseMaterial(material, symbolTable);
                }
            }

            World.Shapes.Add(shape);
        }

        private Matrix ParseTransform(JToken token)
        {
            Matrix transform = Matrix.Identity();

            foreach (var element in token.ToObject<JObject>())
            {
                if (element.Key == "Scale")
                {
                    JToken value = element.Value;
                    transform = Transformation.Scaling((float)value[0], (float)value[1], (float)value[2]) * transform;
                }
                else if (element.Key == "RotateX")
                {
                    transform = Transformation.RotationX((float)element.Value) * transform;
                }
                else if (element.Key == "RotateY")
                {
                    transform = Transformation.RotationY((float)element.Value) * transform;
                }
                else if (element.Key == "RotateZ")
                {
                    transform = Transformation.RotationZ((float)element.Value) * transform;
                }
                else if (element.Key == "Translation")
                {
                    JToken value = element.Value;
                    transform = Transformation.Translation((float)value[0], (float)value[1], (float)value[2]) * transform;
                }
            }

            return transform;
        }
    }
}
