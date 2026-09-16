namespace Engine.Core;

public sealed class Transform : Component
{
    public float X { get; set; }
    public float Y { get; set; }

    public Transform(GameObject gameObject) : this(gameObject, 0f, 0f)
    {
        
    }
    public Transform(GameObject gameObject, float x, float y) : base(gameObject)
    {
        X = x;
        Y = y;
    }
}