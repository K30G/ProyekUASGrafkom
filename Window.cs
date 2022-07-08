using System;
using System.Collections.Generic;
using System.Text;
using LearnOpenTK.Common;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace UASgrafkom 
{
    // This tutorial is split up into multiple different bits, one for each type of light.

    // The following is the code for the directional light, a light that has a direction but no position.
    public class Window : GameWindow
    {
        private Camera _camera;

        private bool _firstMove = true;
        private Vector2 _lastPos;

        private Objects ground = new Objects();
        private Objects karakter = new Objects();
        private Objects lego = new Objects();
        private Objects alas = new Objects();
        private Objects mage = new Objects();
        private Objects sun = new Objects();


        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            //warna langit
            GL.ClearColor(0.529f, 0.8078f, 0.922f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            ground.LoadObjFile("../../../Resources/Ground.obj");

            sun.LoadMaterial("../../../Resources/moon 2K.mtl");
            sun.LoadObjFile("../../../Resources/moon 2K.obj", "../../../Shaders/shader.frag", false);

            karakter.LoadMaterial("../../../Resources/Witch2.mtl");
            karakter.LoadObjFile("../../../Resources/Witch2.obj");

            alas.LoadMaterial("../../../Resources/stage.mtl");
            alas.LoadObjFile("../../../Resources/stage.obj");


            ground.scale(0.5f);
            ground.translate(0.0f, 0.0f, 2.0f);


            sun.translate(1.2f, 3.0f, 7.0f);


            karakter.rotate(0.0f, -90.0f, 0.0f);
            karakter.translate(0.0f, 0.05f, 2.35f);


            alas.scale(0.08f);
            alas.rotate(0.0f, 90.0f, 0.0f);
            alas.translate(0.0f, 0.05f, 2.35f);
            

            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorGrabbed = true;
            base.OnLoad();
        }

        

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            sun.render(_camera);
            karakter.render(_camera);
            ground.render(_camera);
            alas.render(_camera);
            

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 0.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }

            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Console.WriteLine(_camera.Fov);
            _camera.Fov -= e.OffsetY;
            base.OnMouseWheel(e);

        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
            base.OnResize(e);
        }
    }
}