using Entitas;

public class TilemapSystems : Feature
{
  public TilemapSystems(Contexts contexts) : base("Tilemap Systems")
  {
    Add(new MapGeneratorSystem(contexts));
  }
}