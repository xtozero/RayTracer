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
                            case "Cylinder":
                                ParseCylinderBasedShape<Cylinder>(element, symbolTable);
                                break;
                            case "Cone":
                                ParseCylinderBasedShape<Cone>(element, symbolTable);
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

        private static void DefineMaterial(JToken token, Dictionary<string, object> symbolTable)
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
                case "Ring":
                    {
                        var colors = token["Colors"];
                        pattern = new RingPattern(ReadColor(colors[0]), ReadColor(colors[1]));
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

        private static Type CreateShape<Type>(JToken token, Dictionary<string, object> symbolTable) where Type : Shape, new()
        {
            Type shape = new Type();

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

            return shape;
        }

        private void ParseShape<Type>(JToken token, Dictionary<string, object> symbolTable) where Type : Shape, new()
        {
            Shape shape = CreateShape<Type>(token, symbolTable);

            World.Shapes.Add(shape);
        }

        private void ParseCylinderBasedShape<Type>(JToken token, Dictionary<string, object> symbolTable) where Type :Cylinder, new()
        {
            Type cylinder = CreateShape<Type>(token, symbolTable);

            var min = token["Min"];
            if (min != null)
            {
                cylinder.Minimum = (float)min;
            }

            var max = token["Max"];
            if (max != null)
            {
                cylinder.Maximum = (float)max;
            }

            var closed = token["Closed"];
            if (closed != null)
            {
                cylinder.Closed = (bool)closed;
            }

            World.Shapes.Add(cylinder);
        }

        private static Matrix ParseTransform(JToken token)
        {
            Matrix result = Matrix.Identity();

            foreach (var element in token)
            {
                foreach(var transform in element.ToObject<JObject>())
                {
                    switch (transform.Key)
                    {
                        case "Scale":
                            {
                                JToken value = transform.Value;
                                Tuple scale = ReadPoint(value);
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
                                JToken value = transform.Value;
                                Tuple translation = ReadPoint(value);
                                result = Transformation.Translation(translation.X, translation.Y, translation.Z) * result;
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            return result;
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
