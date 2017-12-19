using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using UnityStandardAssets._2D;


public class PlayerEnterDungeonSystem : ReactiveSystem<GameEntity>
{
  GameContext _context;
  public PlayerEnterDungeonSystem(Contexts contexts) : base(contexts.game)
  {
    _context = contexts.game;
  }

  protected override void Execute(List<GameEntity> entities)
  {
    DungeonComponent dungeon = entities.SingleEntity().dungeon;

    // Initialize player if doesn't exists
    if(!_context.isPlayer){
      _context.isPlayer = true;
      GameObject playerObject = GameObject.Instantiate<GameObject>(_context.playerModel.value.prefab);
      _context.playerEntity.AddView(playerObject);  
			_context.playerEntity.isMoveable = true;
      playerObject.Link(_context.playerEntity, _context);
      Camera.main.GetComponent<Camera2DFollow>().target = playerObject.transform;
    }

    _context.playerEntity.ReplacePosition(dungeon.entry);
    _context.playerEntity.ReplaceGridCoordinate(dungeon.entry);
  }

  protected override bool Filter(GameEntity entity)
  {
    return entity.hasDungeon;
  }

  protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
  {
    return context.CreateCollector(GameMatcher.Dungeon.Added());
  }
}