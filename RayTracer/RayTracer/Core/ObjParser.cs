using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RayTracer
{
    public class ObjParser
    {
        public void ParseObjFile(char[] contents)
        {
            while(IsContentsRemain(contents))
            {
                Token token = GetNextToken(contents);

                switch(token.Type)
                {
                    case TokenType.Vertex:
                        ReadVertex(contents);
                        break;
                    case TokenType.Normal:
                        ReadNormal(contents);
                        break;
                    case TokenType.Face:
                        ReadFace(contents);
                        break;
                    case TokenType.Group:
                        AddGroup(contents);
                        break;
                    default:
                        break;
                }
            }
        }

        public Group ObjToGroup()
        {
            var g = new Group();

            foreach(var group in Groups)
            {
                if (group.Value.Shapes.Count > 0)
                {
                    g.AddChild(group.Value);
                }
            }

            return g;
        }

        private void ReadVertex(char[] contents)
        {
            var vertex = Tuple.Point(0, 0, 0);
            for(int i = 0; i < 3; ++i)
            {
                Token token = GetNextToken(contents);
                if(token.Type == TokenType.Number)
                {
                    vertex[i] = float.Parse(token.Value);
                }
                else
                {
                    throw new FormatException($"{token.Value} is not a Number");
                }
            }

            Vertices.Add(vertex);
        }

        private void ReadNormal(char[] contents)
        {
            var normal = Tuple.Vector(0, 0, 0);
            for (int i = 0; i < 3; ++i)
            {
                Token token = GetNextToken(contents);
                if (token.Type == TokenType.Number)
                {
                    normal[i] = float.Parse(token.Value);
                }
                else
                {
                    throw new FormatException($"{token.Value} is not a Number");
                }
            }

            Normals.Add(normal);
        }

        private void ReadFace(char[] contents)
        {
            List<int>[] faces = new[] { new List<int>(), new List<int>(), new List<int>() };
            FaceType faceType = FaceType.Vertex;
            int totalFaceType = Enum.GetValues(typeof(FaceType)).Length;
            TokenType preTokenType = TokenType.Unrecognized;

            while(IsContentsRemain(contents) &&
                (PeekNextChar(contents) != '\n') && 
                (PeekNextChar(contents) != '\r'))
            {
                Token token = GetNextToken(contents);
                if (token.Type == TokenType.Number)
                {
                    if (preTokenType == TokenType.Number)
                    {
                        faceType = FaceType.Vertex;
                    }

                    int index;
                    if (int.TryParse(token.Value, out index))
                    {
                        faces[(int)faceType].Add(index - 1);
                    }
                    else
                    {
                        throw new FormatException($"{token.Value} is not a Integer");
                    }
                }
                else if (token.Type == TokenType.Slash)
                {
                    ++faceType;
                    faceType = (FaceType)((int)faceType % totalFaceType);
                }
                else
                {
                    throw new FormatException($"{token.Value} is not a Number");
                }

                preTokenType = token.Type;
            }

            var vertexIndices = faces[(int)FaceType.Vertex];
            var texcoordIndicies = faces[(int)FaceType.Texcoord];
            var normaIndices = faces[(int)FaceType.Normal];

            if (normaIndices.Count == 0)
            {
                for (int i = 1, end = vertexIndices.Count - 1; i < end; ++i)
                {
                    curGroup.AddChild(new Triangle(Vertices[vertexIndices[0]], 
                                                    Vertices[vertexIndices[i]], 
                                                    Vertices[vertexIndices[i + 1]]));
                }
            }
            else
            {
                var indexPairs = vertexIndices.Zip(normaIndices, (lhs, rhs) => System.Tuple.Create(lhs, rhs)).ToList();

                for (int i = 1, end = indexPairs.Count - 1; i < end; ++i)
                {
                    curGroup.AddChild(new SmoothTriangle(Vertices[indexPairs[0].Item1],
                                                        Vertices[indexPairs[i].Item1],
                                                        Vertices[indexPairs[i + 1].Item1],
                                                        Normals[indexPairs[0].Item2],
                                                        Normals[indexPairs[i].Item2],
                                                        Normals[indexPairs[i + 1].Item2]));
                }
            }
        }

        private void AddGroup(char[] contents)
        {
            Token token = GetNextToken(contents);
            if (Groups.ContainsKey(token.Value) == false)
            {
                Groups.Add(token.Value, new Group());
            }
            curGroup = Groups[token.Value];
        }

        private char GetNextChar(char[] contents)
        {
            return contents[curPos++];
        }

        private char PeekNextChar(char[] contents)
        {
            return contents[curPos];
        }

        private Token GetNextToken(char[] contents)
        {
            SkipWhiteSpace(contents);
            
            var token = new Token();
            var tempValue = new StringBuilder();
            while (IsContentsRemain(contents))
            {
                char c = PeekNextChar(contents);
                if (IsWhiteSpace(c))
                {
                    break;
                }
                else if (c == '/')
                {
                    if (tempValue.Length == 0)
                    {
                        tempValue.Append(c);
                        ++curPos;
                    }
                    break;
                }
                else
                {
                    tempValue.Append(c);
                    ++curPos;
                }
            }

            token.Value = tempValue.ToString();

            if (token.Value == "v")
            {
                token.Type = TokenType.Vertex;
            }
            else if (token.Value == "vn")
            {
                token.Type = TokenType.Normal;
            }
            else if (token.Value == "f")
            {
                token.Type = TokenType.Face;
            }
            else if (token.Value == "g")
            {
                token.Type = TokenType.Group;
            }
            else if (token.Value == "/")
            {
                token.Type = TokenType.Slash;
            }
            else if (float.TryParse(token.Value, out _))
            {
                token.Type = TokenType.Number;
            }
            else
            {
                token.Type = TokenType.Unrecognized;
            }

            return token;
        }

        private void SkipWhiteSpace(char[] contents)
        {
            while(IsContentsRemain(contents))
            {
                char nextChar = contents[curPos];
                if (IsWhiteSpace(nextChar))
                {
                    ++curPos;
                }
                else
                {
                    break;
                }
            }
        }

        private static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\n' || c == '\r';
        }

        private bool IsContentsRemain(char[] contents)
        {
            return curPos < contents.GetLength(0);
        }

        private enum TokenType
        {
            Vertex,
            Normal,
            Face,
            Group,
            Slash,
            Number,
            Unrecognized
        };

        private enum FaceType
        {
            Vertex,
            Texcoord,
            Normal
        }

        private struct Token
        {
            public TokenType Type { get; set; }
            public string Value { get; set; }
        }

        public ObjParser()
        {
            Groups.Add("default", new Group());
            curGroup = Groups["default"];
        }

        private int curPos = 0;
        public List<Tuple> Vertices { get; private set; } = new List<Tuple>();
        public List<Tuple> Normals { get; private set; } = new List<Tuple>();
        public Dictionary<string, Group> Groups { get; private set; } = new Dictionary<string, Group>();
        private Group curGroup;
    }
}
