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
    class Material
    {
        public Vector3 color = new Vector3();
        public string name = "";
        public void setmaterial(string _name, Vector3 _color)
        {
            name = _name;
            color = _color;
        }
    }
}
