using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace Triangle_Drawing_Window
{
    internal class Game : GameWindow
    {


        float[] vertices =
        {
            0f,0.5f,0f, //top vertex
            -0.5f,-0.5f, 0f, //bottom  left vertex
            0.5f, -0.5f, 0f //bottom right vertex
        };



        //Render Pipeline variables
        int vao;
        int shaderProgram;


        //CONSTANTS
        int width, height;


        //Creating the window
        public Game(int width, int height) : base(GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            //Center the window
            this.CenterWindow(new Vector2i(width, height));

            this.width = width;
            this.height = height;
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);

            this.width = e.Width;
            this.height = e.Height;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            //Change title
            Title = "Triangle Window";

            vao = GL.GenVertexArray();

            int vbo = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //bind the vao
            GL.BindVertexArray(vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexArrayAttrib(vao, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0); //unbinding vbo
            GL.BindVertexArray(0);

            //create shader program
            shaderProgram = GL.CreateProgram();

            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, LoadShaderSource("Default.vert"));
            GL.CompileShader(vertexShader);

            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, LoadShaderSource("Default.frag"));
            GL.CompileShader(fragmentShader);

            GL.AttachShader(shaderProgram, vertexShader);
            GL.AttachShader(shaderProgram, fragmentShader);


            GL.LinkProgram(shaderProgram);

            //Delete the shaders
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);


        }


        protected override void OnUnload()
        {
            base.OnUnload();

            GL.DeleteVertexArray(vao);
            GL.DeleteProgram(shaderProgram);
        }

        //Is in charge of all the rendenring
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            //setting up the screen color
            GL.ClearColor(0.6f, 0.3f, 1f, 1f);

            //applying the color
            GL.Clear(ClearBufferMask.ColorBufferBit);


            //Draw triangle
            GL.UseProgram(shaderProgram);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);


            Context.SwapBuffers();

            //Renders
            base.OnRenderFrame(args);


        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        public static string LoadShaderSource(string filePath)
        {
            string shaderSource = "";

            try
            {
                using (StreamReader reader = new StreamReader("../../../Shaders/" + filePath))
                {
                    shaderSource = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to load shader source file:" + e.Message);
            }

            return shaderSource;
        }

    }
}
