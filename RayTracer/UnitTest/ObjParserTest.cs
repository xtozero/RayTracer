using RayTracer;
using System.IO;
using Xunit;
using static System.Environment;

namespace UnitTest
{
    public static class ObjParserTest
    {
        [Fact]
        private static void TestCase01()
        {
            // Ignoring unrecongnized lines
            var contents = "There was a young lady named Bright" + NewLine +
                           "who traveled much faster than light." + NewLine +
                           "She set out one day" + NewLine +
                           "in a relative way" + NewLine +
                           "and came back the previous night.";
            var parser = new ObjParser();
            parser.ParseObjFile(contents.ToCharArray());

            Assert.Empty(parser.Vertices);
        }

        [Fact]
        private static void TestCase02()
        {
            // Vertex records
            var contents = "v -1 1 0" + NewLine +
                           "v -1.0000 0.5000 0.0000" + NewLine +
                           "v 1 0 0" + NewLine +
                           "v 1 1 0";
            var parser = new ObjParser();
            parser.ParseObjFile(contents.ToCharArray());

            Assert.Equal(4, parser.Vertices.Count);
            Assert.Equal(Tuple.Point(-1, 1, 0), parser.Vertices[0]);
            Assert.Equal(Tuple.Point(-1, 0.5f, 0), parser.Vertices[1]);
            Assert.Equal(Tuple.Point(1, 0, 0), parser.Vertices[2]);
            Assert.Equal(Tuple.Point(1, 1, 0), parser.Vertices[3]);
        }

        [Fact]
        private static void TestCase03()
        {
            // Parsing triangle faces
            var contents = "v -1 1 0" + NewLine +
                           "v -1 0 0" + NewLine +
                           "v 1 0 0" + NewLine +
                           "v 1 1 0" + NewLine +
                           NewLine +
                           "f 1 2 3" + NewLine +
                           "f 1 3 4";

            var parser = new ObjParser();
            parser.ParseObjFile(contents.ToCharArray());
            Group g = parser.Groups["default"];
            
            Assert.Equal(2, g.Shapes.Count);

            Triangle t1 = g.Shapes[0] as Triangle;
            Triangle t2 = g.Shapes[1] as Triangle;

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
        }

        [Fact]
        private static void TestCase04()
        {
            // Parsing triangle faces
            var contents = "v -1 1 0" + NewLine +
                           "v -1 0 0" + NewLine +
                           "v 1 0 0" + NewLine +
                           "v 1 1 0" + NewLine +
                           "v 0 2 0" + NewLine +
                           NewLine +
                           "f 1 2 3 4 5";

            var parser = new ObjParser();
            parser.ParseObjFile(contents.ToCharArray());
            Group g = parser.Groups["default"];

            Assert.Equal(3, g.Shapes.Count);

            Triangle t1 = g.Shapes[0] as Triangle;
            Triangle t2 = g.Shapes[1] as Triangle;
            Triangle t3 = g.Shapes[2] as Triangle;

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.NotNull(t3);
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Vertices[0], t2.P1);
            Assert.Equal(parser.Vertices[2], t2.P2);
            Assert.Equal(parser.Vertices[3], t2.P3);
            Assert.Equal(parser.Vertices[0], t3.P1);
            Assert.Equal(parser.Vertices[3], t3.P2);
            Assert.Equal(parser.Vertices[4], t3.P3);
        }

        [Fact]
        private static void TestCase05()
        {
            // Triangles in groups
            using (StreamReader sr = File.OpenText(@"../mesh/triangles.obj"))
            {
                string contents = sr.ReadToEnd();
                var parser = new ObjParser();
                parser.ParseObjFile(contents.ToCharArray());
                Group g1 = parser.Groups["FirstGroup"];
                Group g2 = parser.Groups["SecondGroup"];

                Assert.NotNull(g1);
                Assert.NotNull(g2);

                Triangle t1 = g1.Shapes[0] as Triangle;
                Triangle t2 = g2.Shapes[0] as Triangle;

                Assert.NotNull(t1);
                Assert.NotNull(t2);
                Assert.Equal(parser.Vertices[0], t1.P1);
                Assert.Equal(parser.Vertices[1], t1.P2);
                Assert.Equal(parser.Vertices[2], t1.P3);
                Assert.Equal(parser.Vertices[0], t2.P1);
                Assert.Equal(parser.Vertices[2], t2.P2);
                Assert.Equal(parser.Vertices[3], t2.P3);
            }
        }

        [Fact]
        private static void TestCase06()
        {
            // Converting an OBJ file to a group
            using (StreamReader sr = File.OpenText(@"../mesh/triangles.obj"))
            {
                string contents = sr.ReadToEnd();
                var parser = new ObjParser();
                parser.ParseObjFile(contents.ToCharArray());
                Group g = parser.ObjToGroup();

                Assert.Contains(parser.Groups["FirstGroup"], g.Shapes);
                Assert.Contains(parser.Groups["SecondGroup"], g.Shapes);
            }
        }

        [Fact]
        private static void TestCase07()
        {
            // Vertex records
            var contents = "vn 0 0 1" + NewLine +
                           "vn 0.707 0 -0.707" + NewLine +
                           "vn 1 2 3";
            var parser = new ObjParser();
            parser.ParseObjFile(contents.ToCharArray());

            Assert.Equal(3, parser.Normals.Count);
            Assert.Equal(Tuple.Vector(0, 0, 1), parser.Normals[0]);
            Assert.Equal(Tuple.Vector(0.707f, 0, -0.707f), parser.Normals[1]);
            Assert.Equal(Tuple.Vector(1, 2, 3), parser.Normals[2]);
        }

        [Fact]
        private static void TestCase08()
        {
            // Faces with normals
            var contents = "v 0 1 0" + NewLine +
                           "v -1 0 0" + NewLine +
                           "v 1 0 0" + NewLine +
                           NewLine +
                           "vn -1 0 0" + NewLine +
                           "vn 1 0 0" + NewLine +
                           "vn 0 1 0" + NewLine +
                           NewLine +
                           "f 1//3 2//1 3//2" + NewLine +
                           "f 1/0/3 2/102/1 3/14/2";
            var parser = new ObjParser();
            parser.ParseObjFile(contents.ToCharArray());

            Group g = parser.Groups["default"];

            Assert.Equal(2, g.Shapes.Count);

            SmoothTriangle t1 = g.Shapes[0] as SmoothTriangle;
            SmoothTriangle t2 = g.Shapes[1] as SmoothTriangle;

            Assert.NotNull(t1);
            Assert.NotNull(t2);
            Assert.Equal(parser.Vertices[0], t1.P1);
            Assert.Equal(parser.Vertices[1], t1.P2);
            Assert.Equal(parser.Vertices[2], t1.P3);
            Assert.Equal(parser.Normals[2], t1.N1);
            Assert.Equal(parser.Normals[0], t1.N2);
            Assert.Equal(parser.Normals[1], t1.N3);
            Assert.Equal(t1, t2);
        }
    }
}
