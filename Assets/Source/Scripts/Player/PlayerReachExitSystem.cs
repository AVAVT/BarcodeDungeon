using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class PlayerReachExitSystem : ReactiveSystem<GameEntity> {

  GameContext _context;
  public PlayerReachExitSystem(Contexts contexts) : base(contexts.game){
    _context = contexts.game;
  }
  
  protected override void Execute(List<GameEntity> entities)
  {
    _context.ReplaceGenerator(_context.generator.seed + 1);
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.isPlayer && entity.hasPosition && entity.position.value == _context.dungeon.exit;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Player, GameMatcher.Position));
  }
}