namespace Engine.Core;

using System.Diagnostics;

public class EngineRuntime
{
    public Scene CurrentScene { get; }

    public EngineRuntime(Scene scene)
    {
        CurrentScene = scene;
    }

    public void Tick(double deltaTime)
    {
        CurrentScene.Update(deltaTime);
    }

    public void Run(int frameCount)
    {
        int currentFrame = 0;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        double previousTime = stopwatch.Elapsed.TotalSeconds;
        while (currentFrame < frameCount)
        {
            double currentTime = stopwatch.Elapsed.TotalSeconds;
            double deltaTime = currentTime - previousTime;
            previousTime = currentTime;
            Tick(deltaTime);
            currentFrame++;
        }
    }
}