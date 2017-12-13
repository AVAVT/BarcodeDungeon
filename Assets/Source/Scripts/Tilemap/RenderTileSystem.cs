using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class RenderTileSystem : ReactiveSystem<GameEntity>
{
  GameContext _context;
  public RenderTileSystem(Contexts contexts) : base(contexts.game)
  {
    _context = contexts.game;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.Tile);
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.hasTile;
  }

  protected override void Execute(List<GameEntity> entities)
  {
    foreach (GameEntity e in entities)
    {
      switch(e.tile.type){
        case TileType.Floor:
          e.ReplaceSprite(_context.tileModels.value.floor);
          break;
        case TileType.Wall:
          e.ReplaceSprite(_context.tileModels.value.wallTop);
          break;
        case TileType.Empty:
        default:
          e.ReplaceSprite(_context.tileModels.value.blackTile);
          break;
      }
    }
  }
}