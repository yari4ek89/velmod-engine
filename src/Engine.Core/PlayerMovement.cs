namespace Engine.Core;

public class PlayerMovement : Component, IUpdatable
{
    public float Speed { get; set; } = 100f;
    
    public PlayerMovement(GameObject gameObject)
        : base(gameObject)
    {
        
    }
    
    public void Update(double deltaTime)
    {
        GameObject.Transform.X += Speed * deltaTime;
    }
}