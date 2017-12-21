using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using System.Linq;

public class ChangeToPlayerTurnSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    GameContext _context;
    IGroup<GameEntity> enemyGroup;
    public ChangeToPlayerTurnSystem(Contexts contexts) : base(contexts.game)
    {
        _context = contexts.game;
    }

    #region IInitializeSystem implementation

    public void Initialize()
    {
        enemyGroup = _context.GetGroup(GameMatcher.Enemy);
    }

    #endregion

    #region implemented abstract members of ReactiveSystem
    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Enemy, GameMatcher.MoveCompleted));
    }
    protected override bool Filter(GameEntity entity)
    {
        return entity.isEnemy && entity.isMoveCompleted;

    }
    protected override void Execute(List<GameEntity> entities)
    {
        if (enemyGroup.GetEntities().All(enemy => enemy.isMoveCompleted))
        {
            _context.turnEntity.ReplaceTurn(TurnType.Player);
        }
    }
    #endregion
}