namespace Engine.Core;

public class GameObject
{
    public string Name { get; }
    public Transform Transform { get; }
    private readonly List<Component> _components = new List<Component>();
    public IReadOnlyList<Component> Components => _components;

    public GameObject(string name)
    {
        Name = name;
        Transform = new Transform(this);
        _components.Add(Transform);
    }

    public T? GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component is T found)
            {
                return found;
            }
        }

        return null;
    }

    public T AddComponent<T>(Func<GameObject, T> creator) where T : Component
    {
        if (typeof(T) == typeof(Transform))
        {
            throw new InvalidOperationException("GameObject already has a Transform.");
        }

        if (GetComponent<T>() != null)
        {
            throw new InvalidOperationException("GameObject already has a component of this type.");
        }
        
        T component = creator(this);
        _components.Add(component);
        return component;
    }

    public T? RemoveComponent<T>() where T : Component
    {
        if (typeof(T) == typeof(Transform))
        {
            throw new InvalidOperationException("GameObject can't remove a Transform.");
        }

        for (int i = 0; i < _components.Count; i++)
        {
            if(_components[i] is T found)
            {
                _components.RemoveAt(i);
                return found;
            }
        }
        return null;
    }

    public void Update(double deltaTime)
    {
        foreach (var component in _components)
        {
            if (component is IUpdatable found)
            {
                found.Update(deltaTime);
            }
        }
    }
}