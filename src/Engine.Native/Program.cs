using System.Drawing;
using Engine.Core;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.OpenGL;

namespace Engine.Native;

static class Program
{
    private static GL _gl = null!;
    private static IWindow _window = null!;
    
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

    static void OnLoad()
    {
        _gl = _window.CreateOpenGL();
    }

    static void OnRender(double deltaTime)
    {
        _gl.ClearColor(Color.CornflowerBlue);
        _gl.Clear(ClearBufferMask.ColorBufferBit);
    }
}