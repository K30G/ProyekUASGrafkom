using LearnOpenTK.Common;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenTK.Graphics.OpenGL4;
using System.Globalization;

namespace UASgrafkom 
{
    class Objects
    {
        private List<Mesh> obj = new List<Mesh>();
        private List<Material> mtl = new List<Material>();
        private int vIndex = 0, vCounter = 0;
        private int vnIndex = 0, vnCounter = 0;

        public void rotate(float x, float y, float z)
        {
            foreach (var meshobj in obj)
            {
                meshobj.rotate(x, y, z);
            }
        }
        public void scale(float scale)
        {
            foreach (var meshobj in obj)
            {
                meshobj.scale(scale);
            }
        }
        public void translate(float x, float y, float z)
        {
            foreach (var meshobj in obj)
            {
                meshobj.translate(x, y, z);
            }
        }

        public void setShininess(float f)
        {
            foreach (var meshobj in obj)
            {
                meshobj.setShininess(f);
            }
        }
        public float FixedStringToDouble(string input)
        {
            float processed = float.Parse(input, CultureInfo.InvariantCulture);
            return processed;
        }
        public void LoadMaterial(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }
            using (StreamReader streamReader = new StreamReader(path))
            {
                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    words.RemoveAll(s => s == string.Empty);
                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);

                    switch (type)
                    {
                        case "newmtl":
                            mtl.Add(new Material());
                            mtl[mtl.Count - 1].name = words[0];
                            break;
                        case "kd":
                            mtl[mtl.Count - 1].color = new Vector3(FixedStringToDouble(words[0]), FixedStringToDouble(words[1]), FixedStringToDouble(words[2]));
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void LoadObjFile(string path, string frag = "../../../Shaders/lighting.frag", bool _shading = true)
        {
           
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Unable to open \"" + path + "\", does not exist.");
            }
            using (StreamReader streamReader = new StreamReader(path))
            {

                while (!streamReader.EndOfStream)
                {
                    List<string> words = new List<string>(streamReader.ReadLine().ToLower().Split(' '));
                    words.RemoveAll(s => s == string.Empty);
                    if (words.Count == 0)
                        continue;

                    string type = words[0];
                    words.RemoveAt(0);


                    switch (type)
                    {
                        case "o":
                            if(obj.Count > 0)
                            {
                                obj[obj.Count - 1].SetUpVertices(frag, _shading);
                            }
                            obj.Add(new Mesh());
                            vIndex = vCounter;
                            vnIndex = vnCounter;
                            break;
                        case "v":
                            obj[obj.Count - 1].tempvertices.Add(new Vector3(FixedStringToDouble(words[0]) / 10, FixedStringToDouble(words[1]) / 10,
                                                            FixedStringToDouble(words[2]) / 10));
                            vCounter++;
                            //vertices.Add(FixedStringToDouble(words[0]) / 10);
                            //vertices.Add(FixedStringToDouble(words[1]) / 10);
                            //vertices.Add(FixedStringToDouble(words[2]) / 10);
                            break;

                        //case "vt":
                        //    textureVertices.Add(new Vector3d(FixedStringToDouble(words[0]), FixedStringToDouble(words[1]),
                        //                                    words.Count < 3 ? 0 : FixedStringToDouble(words[2])));
                        //    break;

                        case "vn":
                            obj[obj.Count - 1].tempnormals.Add(new Vector3(FixedStringToDouble(words[0]), FixedStringToDouble(words[1]), FixedStringToDouble(words[2])));
                            vnCounter++;
                            break;

                        case "f":
                            foreach (string w in words)
                            {
                                if (w.Length == 0)
                                    continue;
                                string[] comps = w.Split('/');

                                obj[obj.Count - 1].tempf.Add(new Vector3i(int.Parse(comps[0]) - 1 - vIndex, 0, int.Parse(comps[2]) - 1 - vnIndex));
                            }
                            break;
                        case "usemtl":
                            foreach (Material mat in mtl)
                            {
                                if (mat.name == words[0])
                                {
                                    obj[obj.Count - 1].color = mat.color;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                if (obj.Count > 0)
                {
                    obj[obj.Count - 1].SetUpVertices(frag, _shading);
                }
            }
        }

        public void render(Camera _camera)
        {
            foreach(Mesh child in obj)
            {
                child.render(_camera);
            }
        }
    }
}
