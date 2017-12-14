using System.Collections.Generic;
using Entitas;
using UnityEngine;


public class PoolReuseSystem : ReactiveSystem<GameEntity>
{
  public PoolReuseSystem(Contexts contexts) : base(contexts.game){

  }
  protected override void Execute(List<GameEntity> entities)
  {
    entities.ForEach(entity => entity.view.gameObject.SetActive(true));
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.isPoolable && !entity.isReusable && entity.hasView;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Poolable, GameMatcher.View).NoneOf(GameMatcher.Reusable));
  }
}