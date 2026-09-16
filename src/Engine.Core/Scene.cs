namespace Engine.Core;

public class Scene
{
    public string Name { get; }
    private readonly List<GameObject> _gameObjects = new List<GameObject>();
    public IReadOnlyList<GameObject> GameObjects => _gameObjects;
    
    public Scene(string name)
    {
        Name = name;
    }
    
    public void Add(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }

    public bool Remove(GameObject gameObject)
    {
        return _gameObjects.Remove(gameObject);
    }

    public void Update(float deltaTime)
    {
        foreach (var gameObject in _gameObjects)
        {
            gameObject.Update(deltaTime);
        }
    }
}