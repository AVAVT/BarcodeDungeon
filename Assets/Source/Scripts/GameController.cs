using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public TileModels tileModels;
  public DungeonSettingModels dungeonSettingModels;
  private Systems _systems;
  private Contexts _contexts;

  void Start()
  {
    _contexts = SetContextsData(Contexts.sharedInstance);
    _systems = CreateSystems(_contexts);
    _systems.Initialize();
  }

  void Update()
  {
    _systems.Execute();
    _systems.Cleanup();
  }

  private static Systems CreateSystems(Contexts contexts)
  {
    return new Feature("Systems")
        .Add(new CoreSystems(contexts))
        .Add(new DungeonGeneratorSystems(contexts));
  }

  private Contexts SetContextsData(Contexts contexts)
  {
    contexts.game.SetTileModels(tileModels);
    contexts.game.SetDungeonSettingModels(dungeonSettingModels);

    return contexts;
  }
}