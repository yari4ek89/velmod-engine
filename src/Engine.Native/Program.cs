using System.Drawing;
using System.IO;
using Engine.Core;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine.Native;

static class Program
{
    private static GL _gl = null!;
    private static IWindow _window = null!;
    private static uint _vao;
    private static uint _shaderProgram;
    
    static void Main()
    {
        Scene firstLevel = new Scene("First Level");
        GameObject player = new GameObject("Player");
        PlayerMovement playerMovement = player.AddComponent(x => new PlayerMovement(x));
        firstLevel.Add(player);
        EngineRuntime engineRuntime = new EngineRuntime(firstLevel);
        
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "Velmod Engine"
        };
        _window = Window.Create(options);
        _window.Update += x => OnUpdate(x, engineRuntime);
        Console.WriteLine(player.Transform.X);
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Run();
        Console.WriteLine(player.Transform.X);
    }

    static void OnUpdate(double deltaTime, EngineRuntime engineRuntime)
    {
        engineRuntime.Tick(deltaTime);
    }

    static unsafe void OnLoad()
    {
        _gl = _window.CreateOpenGL();
        _gl.ClearColor(Color.CornflowerBlue);

        float[] vertices =
        {
            0.0f, 0.5f,
            -0.5f, -0.5f,
            0.5f, -0.5f
        };

        uint vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, vbo);
        _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(sizeof(float) * vertices.Length), vertices, BufferUsageARB.StaticDraw);

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        _gl.VertexAttribPointer(0u, 2, VertexAttribPointerType.Float, false, (uint)(2 * sizeof(float)), (void*)(0));
        _gl.EnableVertexAttribArray(0);

        string vertexSource = File.ReadAllText(@"/mnt/hdd2/ugc-platform/src/Engine.Native/Shaders/basic.vert");
        string fragmentSource = File.ReadAllText(@"/mnt/hdd2/ugc-platform/src/Engine.Native/Shaders/basic.frag");

        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        
        _gl.ShaderSource(vertexShader, vertexSource);
        _gl.ShaderSource(fragmentShader, fragmentSource);
        
        _gl.CompileShader(vertexShader);
        _gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vertexStatus);        
        _gl.CompileShader(fragmentShader);
        _gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fragmentStatus);

        if (vertexStatus == 0)
        {
            string log = _gl.GetShaderInfoLog(vertexShader);
            Console.WriteLine(log);
        }
        if (fragmentStatus == 0)
        {
            string log = _gl.GetShaderInfoLog(fragmentShader);
            Console.WriteLine(log);
        }

        _shaderProgram = _gl.CreateProgram();
        _gl.AttachShader(_shaderProgram, vertexShader);
        _gl.AttachShader(_shaderProgram, fragmentShader);
        _gl.LinkProgram(_shaderProgram);
        _gl.GetProgram(_shaderProgram, GLEnum.LinkStatus, out int programStatus);

        if (programStatus == 0)
        {
            string log = _gl.GetProgramInfoLog(_shaderProgram);
            Console.WriteLine(log);
        }
    }

    static void OnRender(double deltaTime)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _gl.UseProgram(_shaderProgram);
        _gl.BindVertexArray(_vao);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }
}