namespace Engine.Core;

public class SpriteRenderer : Component
{
    public string SpritePath { get; set; }
    public bool Visible { get; set; }
    
    public SpriteRenderer(GameObject gameObject, string spritePath, bool visible = true)
        : base(gameObject)
    {
        SpritePath = spritePath;
        Visible = visible;
    }
}