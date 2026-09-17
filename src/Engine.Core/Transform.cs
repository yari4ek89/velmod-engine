namespace Engine.Core;

public sealed class Transform : Component
{
    public double X { get; set; }
    public double Y { get; set; }

    public Transform(GameObject gameObject) : this(gameObject, 0f, 0f)
    {
        
    }
    public Transform(GameObject gameObject, double x, double y) : base(gameObject)
    {
        X = x;
        Y = y;
    }
}