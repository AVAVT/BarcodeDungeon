using Entitas;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TKLibs;
using Entitas.Unity;

public class MapGeneratorSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
  GameContext _context;
  DungeonSettingModels _settingModels;

  IGroup<GameEntity> _tiles;
  public MapGeneratorSystem(Contexts contexts) : base(contexts.game)
  {
    _context = contexts.game;
    _settingModels = _context.dungeonSettingModels.value;
    _tiles = _context.GetGroup(GameMatcher.Tile);
  }

  public void Initialize()
  {
    // TODO create procedural seed
    // _context.SetGenerator((int)System.DateTime.Now.Ticks);
    // _context.SetGenerator(6);
  }

  protected override void Execute(List<GameEntity> entities)
  {
    CreateDungeonWithSeed(_context.generator.seed);
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.hasGenerator;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Generator));
  }

  private void CreateDungeonWithSeed(int seed)
  {
    System.Random rng = new System.Random(seed);
    Dictionary<Vector2Int, TileType> map = MapGenerator.GenerateMap(_settingModels.sizes, rng);
    
    GameEntity[] tiles = _tiles.GetEntities();
    for(int i = tiles.Length - 1; i >= 0; i--){
      GameEntity tile = tiles[i];
      tile.view.gameObject.Unlink();
      GameObject.Destroy(tile.view.gameObject);
      tile.Destroy();
    }

    _context.ReplaceDungeon(
      map,
      Vector2Int.zero,
      new Vector2Int(_settingModels.sizes.x*2-2, _settingModels.sizes.y*2-2)
    );

    IEnumerable<int> xCoords = Enumerable.Range(0, _settingModels.sizes.x * 2 - 1);
    IEnumerable<int> yCoords = Enumerable.Range(0, _settingModels.sizes.y * 2 - 1);
    foreach (int x in xCoords)
    {
      foreach (int y in yCoords)
      {
        Vector2Int coord = new Vector2Int(x, y);
        GameEntity tile = _context.CreateEntity();
        tile.AddPosition(coord);

        if (_context.dungeon.map.ContainsKey(coord))
        {
          tile.AddTile(_context.dungeon.map[coord]);
        }
        else
        {
          tile.AddTile(TileType.Empty);
        }
      }
    }
  }
}