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
    private static ShaderProgram _shaderProgram = null!;
    private static Renderer2D _renderer2D = null!;
    
    static void Main()
    {
        Scene firstLevel = new Scene("First Level");
        
        GameObject player = new GameObject("Player");
        PlayerMovement playerMovement = player.AddComponent(x => new PlayerMovement(x));
        playerMovement.Speed = 100f;
        SpriteRenderer spriteRenderer = player.AddComponent(x => new SpriteRenderer(x, "/mnt/hdd2/ugc-platform/src/Engine.Native/test.png"));
        player.Transform.X = 200f;
        player.Transform.Y = 300f;
        
        GameObject enemy = new GameObject("Enemy");
        PlayerMovement enemyMovement = enemy.AddComponent(x => new PlayerMovement(x));
        enemyMovement.Speed = 50f;
        SpriteRenderer spriteRenderer1 = enemy.AddComponent(x => new SpriteRenderer(x, "/mnt/hdd2/ugc-platform/src/Engine.Native/test.png"));
        enemy.Transform.X = 600f;
        enemy.Transform.Y = 300f;
        
        firstLevel.Add(player);
        firstLevel.Add(enemy);
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
        _window.Render += x => OnRender(x, firstLevel);
        _window.Run();
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
        _renderer2D = new Renderer2D(_gl, _shaderProgram);
        _shaderProgram.ShaderProcess("/mnt/hdd2/ugc-platform/src/Engine.Native/Shaders/basic.vert", 
            "/mnt/hdd2/ugc-platform/src/Engine.Native/Shaders/basic.frag");
    }

    static void OnRender(double deltaTime, Scene scene)
    {
        _renderer2D.Render(_window.Size.X, _window.Size.Y, scene);
    }
}