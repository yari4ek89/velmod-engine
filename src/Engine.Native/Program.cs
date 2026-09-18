using System.Drawing;
using Engine.Core;
using Engine.Native.Rendering;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine.Native;

static class Program
{
    private static GL _gl = null!;
    private static IWindow _window = null!;
    private static uint _vao;
    private static ShaderProgram _shaderProgram = null!;
    
    static void Main()
    {
        Scene firstLevel = new Scene("First Level");
        GameObject player = new GameObject("Player");
        PlayerMovement playerMovement = player.AddComponent(x => new PlayerMovement(x));
        playerMovement.Speed = 0.1f;
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
        _window.Render += x => OnRender(x, player);
        _window.Run();
        Console.WriteLine(player.Transform.X);
    }

    static void OnUpdate(double deltaTime, EngineRuntime engineRuntime)
    {
        engineRuntime.Tick(deltaTime);
    }
    
    static void OnLoad()
    {
        _gl = _window.CreateOpenGL();
        _gl.ClearColor(Color.CornflowerBlue);
        _shaderProgram = new ShaderProgram(_gl);
        GeometryProcessing();
        _shaderProgram.ShaderProcess("/mnt/hdd2/ugc-platform/src/Engine.Native/Shaders/basic.vert", 
            "/mnt/hdd2/ugc-platform/src/Engine.Native/Shaders/basic.frag");
    }

    static void OnRender(double deltaTime, GameObject player)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        
        _shaderProgram.Use();
        _shaderProgram.SetVector2(
            "uPosition",
            (float)player.Transform.X,
            (float)player.Transform.Y
            );
        
        _gl.BindVertexArray(_vao);
        _gl.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    static unsafe void GeometryProcessing()
    {
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
    }
}