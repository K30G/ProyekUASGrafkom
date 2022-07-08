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
    class Mesh
    {
        //Vector 3 pastikan menggunakan OpenTK.Mathematics
        //tanpa protected otomatis komputer menganggap sebagai private
         public List<float> vertices = new List<float>
         {
             -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f, 
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  

            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f, 

            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f, 
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f, 
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  

             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f, 
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  

            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f, 
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  

            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f, 
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f, 
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f
            };
         
         protected List<Vector3d> textureVertices = new List<Vector3d>();
         protected List<Vector3d> normals = new List<Vector3d>();
         protected List<uint> vertexIndices = new List<uint>();
         protected int _vertexBufferObject;
         //protected int _elementBufferObject;
         protected int _vertexArrayObject;
         protected Shader _lightingShader;
         public Vector3 color = new Vector3(0.5f, 0.0f, 0.0f);
         protected Matrix4 transform;
         protected int counter = 0;
         public List<Mesh> child = new List<Mesh>();

        public List<Vector3> tempvertices = new List<Vector3>();
        public List<Vector3> tempnormals = new List<Vector3>();
        public List<Vector3i> tempf = new List<Vector3i>();

        public float shininess = 32.0f;

        private bool shading = true;
        public Mesh()
        {
        }
        
       
        public void setupObject(string frag, bool _shading)
        {
            shading = _shading;
            _lightingShader = new Shader("../../../Shaders/shader.vert", frag);
            transform = Matrix4.Identity;
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);
            {
                _vertexArrayObject = GL.GenVertexArray();
                GL.BindVertexArray(_vertexArrayObject);

                var positionLocation = _lightingShader.GetAttribLocation("aPos");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

                var normalLocation = _lightingShader.GetAttribLocation("aNormal");
                GL.EnableVertexAttribArray(normalLocation);
                GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

                var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 6 * sizeof(float));
            }
        }
        public void addChild(Mesh _child)
        {
            child.Add(_child);
        }
        public virtual void render(Camera _camera)
        {
            //render itu akan selalu terpanggil setiap frame 
            _lightingShader.Use();

            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            if (shading)
            {
                _lightingShader.SetVector3("viewPos", _camera.Position);
                _lightingShader.SetVector3("material.diffuse", color);
                _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
                _lightingShader.SetFloat("material.shininess", shininess);

                _lightingShader.SetVector3("light.direction", new Vector3(-2.0f, -10.0f, -9.0f));

                _lightingShader.SetVector3("light.ambient", new Vector3(1.5f));
                _lightingShader.SetVector3("light.diffuse", new Vector3(5.0f));
                _lightingShader.SetVector3("light.specular", new Vector3(1.8f));
            }           

            _lightingShader.SetMatrix4("model", transform);

            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count / 6);

            foreach (var meshobj in child)
            {
                meshobj.render(_camera);
            }
        }

        public void setShininess(float _shininess)
        {
            shininess = _shininess;
            _lightingShader.SetFloat("material.shininess", shininess);
        }
        public List<float> getVertices()
        {
            return vertices;
        }
        public List<uint> getVertexIndices()
        {
            return vertexIndices;
        }
        public void setvertices(List<float> vertex)
        {
            vertices = vertex;
        }
        public void setVertexIndices(List<uint> temp)
        {
            vertexIndices = temp;
        }
        public int getVertexBufferObject()
        {
            return _vertexBufferObject;
        }


        public int getVertexArrayObject()
        {
            return _vertexArrayObject;
        }

        public Shader getShader()
        {
            return _lightingShader;
        }

        public Matrix4 getTransform()
        {
            return transform;
        }
        public void rotate(float x, float y, float z)
        {
            transform = transform * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(x));
            transform = transform * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(y));
            transform = transform * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(z));
            foreach (var meshobj in child)
            {
                meshobj.rotate(x, y, z);
            }
        }
        public void scale(float scale)
        {
            transform = transform * Matrix4.CreateScale(scale);
            foreach (var meshobj in child)
            {
                meshobj.scale(scale);
            }
        }
        public void translate(float x, float y, float z)
        {
            transform = transform * Matrix4.CreateTranslation(x, y, z);
            foreach (var meshobj in child)
            {
                meshobj.translate(x, y, z);
            }
        }
        
        
        public float FixedStringToDouble(string input) {
            float processed = float.Parse(input, CultureInfo.InvariantCulture);
            return processed;
        }

        public void SetUpVertices(string frag , bool _shading)
        {
            vertices = new List<float>();
            foreach (Vector3i curr in tempf)
            {
                vertices.Add(tempvertices[curr.X].X);
                vertices.Add(tempvertices[curr.X].Y);
                vertices.Add(tempvertices[curr.X].Z);
                vertices.Add(tempnormals[curr.Z].X);
                vertices.Add(tempnormals[curr.Z].Y);
                vertices.Add(tempnormals[curr.Z].Z);
            }
            setupObject(frag, _shading);
            tempf = null;
            tempnormals = null;
            tempvertices = null;
        }

    }
}
