using Entitas;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TKLibs;
using Entitas.Unity;

public class MapGeneratorSystem : ReactiveSystem<GameEntity>
{
  GameContext _context;
  DungeonSettingModels _settingModels;

  IGroup<GameEntity> _activeTiles;
  IGroup<GameEntity> _reusableTiles;
  public MapGeneratorSystem(Contexts contexts) : base(contexts.game)
  {
    _context = contexts.game;
    _settingModels = _context.dungeonSettingModels.value;
    _activeTiles = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Tile, GameMatcher.Poolable).NoneOf(GameMatcher.Reusable));
    _reusableTiles = _context.GetGroup(GameMatcher.AllOf(GameMatcher.Tile, GameMatcher.Poolable, GameMatcher.Reusable));
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
    
    foreach(GameEntity tile in _activeTiles){
      tile.isReusable = true;
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

        GameEntity tile = _reusableTiles.count > 0 ? _reusableTiles.First() : _context.CreateEntity();
        tile.ReplacePosition(coord);
        tile.isPoolable = true;
        tile.isReusable = false;

        if (_context.dungeon.map.ContainsKey(coord))
        {
          tile.ReplaceTile(_context.dungeon.map[coord]);
        }
        else
        {
          tile.ReplaceTile(TileType.Empty);
        }
      }
    }
  }
}