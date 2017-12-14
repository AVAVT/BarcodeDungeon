using Entitas;

public class DungeonGeneratorSystems : Feature
{
  public DungeonGeneratorSystems(Contexts contexts) : base("Dungeon Generator Systems")
  {
    Add(new MapGeneratorSystem(contexts));
    Add(new BarcodeScanSystem(contexts));
  }
}