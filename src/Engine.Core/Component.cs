namespace Engine.Core;

public class Component
{
    public GameObject GameObject { get; }

    public Component(GameObject gameObject)
    {
        GameObject = gameObject;
    }
}