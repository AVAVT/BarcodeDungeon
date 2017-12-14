using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class PoolCleanupSystem : ReactiveSystem<GameEntity>
{
  public PoolCleanupSystem(Contexts contexts) : base(contexts.game){

  }
  protected override void Execute(List<GameEntity> entities)
  {
    entities.ForEach(entity => entity.view.gameObject.SetActive(false));
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.isPoolable && entity.isReusable && entity.hasView;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Poolable, GameMatcher.Reusable, GameMatcher.View));
  }
}