namespace Engine.Core;

public class EngineRuntime
{
    public Scene CurrentScene { get; }

    public EngineRuntime(Scene scene)
    {
        CurrentScene = scene;
    }

    public void Tick(float deltaTime)
    {
        CurrentScene.Update(deltaTime);
    }
}