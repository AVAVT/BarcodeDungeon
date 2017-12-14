using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class MoveRenderSystem : ReactiveSystem<GameEntity>
{

  public MoveRenderSystem(Contexts contexts) : base(contexts.game){

  }
  protected override void Execute(List<GameEntity> entities)
  {
    entities.ForEach(entity => {
      entity.ReplacePosition(entity.gridCoordinate.value);
    });
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.hasGridCoordinate && entity.hasPosition && entity.position.value != entity.gridCoordinate.value;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.GridCoordinate);
  }
}