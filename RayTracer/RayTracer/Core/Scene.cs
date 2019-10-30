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
                using (JsonTextReader textReader = new JsonTextReader(sr))
                {
                    JArray jScene = (JArray)JToken.ReadFrom(textReader);
                    var symbolTable = new Dictionary<string, object>();

                    foreach (var element in jScene)
                    {
                        string key = (string)element["Class"];
                        switch (key)
                        {
                            case "Camera":
                                ParseCamera(element, symbolTable);
                                break;
                            case "PointLight":
                                ParsePointLight(element, symbolTable);
                                break;
                            case "Material":
                                DefineMaterial(element, symbolTable);
                                break;
                            case "Plane":
                                ParseShape<Plane>(element, symbolTable);
                                break;
                            case "Sphere":
                                ParseShape<Sphere>(element, symbolTable);
                                break;
                            case "Cube":
                                ParseShape<Cube>(element, symbolTable);
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                }
            }
        }

        private void ParseCamera(JToken token, Dictionary<string, object> _)
        {
            int width = (int)token["Width"];
            int height = (int)token["Height"];
            float fov = (float)token["Fov"];

            Camera = new Camera(width, height, fov);

            var from = token["From"];
            var to = token["To"];
            var up = token["Up"];

            Camera.Transform = Transformation.LookAt(ReadPoint(from), ReadPoint(to), ReadVector(up));
        }

        private void ParsePointLight(JToken token, Dictionary<string, object> _)
        {
            var at = token["At"];
            var intensity = token["Intensity"];

            World.Light = new PointLight(ReadPoint(at), ReadColor(intensity));
        }

        private static Material CreateMaterial(JToken token)
        {
            Material m = new Material();

            var color = token["Color"];
            if (color != null)
            {
                m.Color = ReadColor(color);
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
                m.Pattern = CreatePattern(pattern);
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

            return m;
        }

        private void DefineMaterial(JToken token, Dictionary<string, object> symbolTable)
        {
            var name = token["Name"];
            if (name != null)
            {
                symbolTable.Add((string)name, CreateMaterial(token));
            }
            else
            {
                throw new InvalidDataException("To define material must need name");
            }
        }

        private static Pattern CreatePattern(JToken token)
        {
            Pattern pattern = null;

            string type = (string)token["Type"];
            switch (type)
            {
                case "Stripes":
                    {
                        var colors = token["Colors"];
                        pattern = new StripePattern(ReadColor(colors[0]), ReadColor(colors[1]));
                    }
                    break;
                case "Checkers":
                    {
                        var colors = token["Colors"];
                        pattern = new CheckersPattern(ReadColor(colors[0]), ReadColor(colors[1]));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            var transform = token["Transform"];
            if (transform != null)
            {
                pattern.Transform = ParseTransform(transform);
            }

            return pattern;
        }

        private void ParseShape<Type>(JToken token, Dictionary<string, object> symbolTable) where Type : Shape, new()
        {
            Shape shape = new Type();

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
                    shape.Material = CreateMaterial(material);
                }
            }

            World.Shapes.Add(shape);
        }

        private static Matrix ParseTransform(JToken token)
        {
            Matrix transform = Matrix.Identity();

            foreach (var element in token.ToObject<JObject>())
            {
                switch (element.Key)
                {
                    case "Scale":
                        {
                            JToken value = element.Value;
                            Tuple scale = ReadPoint(value);
                            transform = Transformation.Scaling(scale.X, scale.Y, scale.Z) * transform;
                        }
                        break;
                    case "RotateX":
                        {
                            transform = Transformation.RotationX((float)element.Value) * transform;
                        }
                        break;
                    case "RotateY":
                        {
                            transform = Transformation.RotationY((float)element.Value) * transform;
                        }
                        break;
                    case "RotateZ":
                        {
                            transform = Transformation.RotationZ((float)element.Value) * transform;
                        }
                        break;
                    case "Translation":
                        {
                            JToken value = element.Value;
                            Tuple translation = ReadPoint(value);
                            transform = Transformation.Translation(translation.X, translation.Y, translation.Z) * transform;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            return transform;
        }

        private static Tuple ReadPoint(JToken token)
        {
            return Tuple.Point((float)token[0], (float)token[1], (float)token[2]);
        }

        private static Tuple ReadColor(JToken token)
        {
            return Tuple.Color((float)token[0], (float)token[1], (float)token[2]);
        }

        private static Tuple ReadVector(JToken token)
        {
            return Tuple.Vector((float)token[0], (float)token[1], (float)token[2]);
        }
    }
}
