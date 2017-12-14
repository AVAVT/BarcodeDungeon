using Entitas;

public class PlayerSystems : Feature
{
  public PlayerSystems(Contexts contexts) : base("Player Systems")
  {
    Add(new PlayerEnterDungeonSystem(contexts));
    Add(new ControllerSystem(contexts));
    Add(new MoveCommandSystem(contexts));
    Add(new PlayerReachExitSystem(contexts));
  }
}