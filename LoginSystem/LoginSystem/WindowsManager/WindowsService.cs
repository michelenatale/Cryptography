

namespace michele.natale.LoginSystems.Apps;

/// <summary>
/// Superior entry-level class
/// </summary>
public sealed class WindowsService : Dictionary<Type, object>
{
  private readonly static object ServiceLock = new();
  public static WindowsService Instance { get; private set; } = [];

  public void AddService<T>(T implements) where T : class
  {
    lock (ServiceLock)
    {
      this[typeof(T)] = implements;
    }
  }

  public T GetService<T>() where T : class
  {
    object service = null!;
    lock (ServiceLock)
    {
      this.TryGetValue(typeof(T), out service!);
    }
    return (T)service;
  }

}