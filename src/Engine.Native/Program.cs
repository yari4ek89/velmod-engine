using Engine.Core;
using Silk.NET.Maths;

namespace Engine.Native;

using Silk.NET.Windowing;

static class Program
{
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
        IWindow window = Window.Create(options);
        window.Update += x => OnUpdate(x, engineRuntime);
        Console.WriteLine(player.Transform.X);
        window.Run();
        Console.WriteLine(player.Transform.X);
    }

    static void OnUpdate(double deltaTime, EngineRuntime engineRuntime)
    {
        engineRuntime.Tick(deltaTime);
    }
}