using Entitas;

public class DungeonGeneratorSystems : Feature
{
  public DungeonGeneratorSystems(Contexts contexts) : base("Tilemap Systems")
  {
    Add(new MapGeneratorSystem(contexts));
  }
}