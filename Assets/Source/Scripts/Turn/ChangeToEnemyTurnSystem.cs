using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using System.Linq;


public class ChangeToEnemyTurnSystem : ReactiveSystem<GameEntity>,IInitializeSystem
{
	GameContext _context;
	IGroup<GameEntity> playerGroup;
	public ChangeToEnemyTurnSystem(Contexts contexts) : base(contexts.game)
	{
		_context = contexts.game;
	}

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		playerGroup = _context.GetGroup (GameMatcher.Player);
	}

	#endregion

	#region implemented abstract members of ReactiveSystem

	protected override ICollector<GameEntity> GetTrigger (IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.AllOf(GameMatcher.Player,GameMatcher.MoveCompleted));
	}

	protected override bool Filter (GameEntity entity)
	{
		return playerGroup.GetEntities().All(player => player.isMoveCompleted);
	}

	protected override void Execute (List<GameEntity> entities)
	{
		_context.turnEntity.ReplaceTurn (TurnType.Enemy);
	}

	#endregion
}