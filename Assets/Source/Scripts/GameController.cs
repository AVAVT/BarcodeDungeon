using Entitas;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public TileModels tileModels;
  public DungeonSettingModels dungeonSettingModels;
  public PlayerModel playerModel;
  public BarcodeScannerModel barcodeScannerModel;
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
        .Add(new MovementSystems(contexts))
        .Add(new TilemapSystems(contexts))
        .Add(new DungeonGeneratorSystems(contexts))
        .Add(new PlayerSystems(contexts));
  }

  private Contexts SetContextsData(Contexts contexts)
  {
    contexts.game.SetTileModels(tileModels);
    contexts.game.SetDungeonSettingModels(dungeonSettingModels);
    contexts.game.SetPlayerModel(playerModel);
    contexts.input.SetBarcodeScannerModel(barcodeScannerModel);
    
    return contexts;
  }
}