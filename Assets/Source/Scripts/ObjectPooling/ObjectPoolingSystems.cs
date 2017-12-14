using Entitas;

public class ObjectPoolingSystems : Feature
{
  public ObjectPoolingSystems(Contexts contexts) : base("Object Pooling Systems")
  {
    Add(new PoolCleanupSystem(contexts));
    Add(new PoolReuseSystem(contexts));
  }
}