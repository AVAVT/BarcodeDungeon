using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using System.Linq;


public class StartTurnSystem : ReactiveSystem<GameEntity>,IInitializeSystem
{
	GameContext _context;
	IGroup<GameEntity> playerGroup,enemyGroup;

	public StartTurnSystem(Contexts contexts) : base(contexts.game)
	{
		_context = contexts.game;
	}

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		if (!_context.hasTurn) {
			_context.CreateEntity ().AddTurn (TurnType.Player);
		}

		playerGroup = _context.GetGroup (GameMatcher.Player);
		enemyGroup = _context.GetGroup (GameMatcher.Enemy);
	}

	#endregion

	#region implemented abstract members of ReactiveSystem

	protected override ICollector<GameEntity> GetTrigger (IContext<GameEntity> context)
	{
		return context.CreateCollector(GameMatcher.Turn);
	}

	protected override bool Filter (GameEntity entity)
	{
		return entity.hasTurn;
	}

	protected override void Execute (List<GameEntity> entities)
	{
		if (_context.turnEntity.turn.type == TurnType.Enemy) {
			foreach (GameEntity enemy in enemyGroup) {
				enemy.isMoveCompleted = false;
			}
		} else if (_context.turnEntity.turn.type == TurnType.Player) {
			foreach (GameEntity player in playerGroup) {
				player.isMoveCompleted = false;
			}
		};
	}

	#endregion
}