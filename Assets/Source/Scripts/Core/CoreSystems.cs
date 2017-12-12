using Entitas;

public class CoreSystems : Feature
{
  public CoreSystems(Contexts contexts) : base("Core Systems")
  {
    Add(new AddViewSystem(contexts));
    Add(new RenderPositionSystem(contexts));
    Add(new RenderSpriteSystem(contexts));
  }
}