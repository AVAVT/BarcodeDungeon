using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class MoveCommandSystem : ReactiveSystem<InputEntity>{
  GameContext _gameContext;
  public MoveCommandSystem(Contexts contexts) : base(contexts.input){
    _gameContext = contexts.game;
  }

  protected override void Execute(List<InputEntity> entities)
  {
    Vector2Int direction = entities.SingleEntity().command.direction;

    if(IsValidMove(_gameContext.dungeon.map, _gameContext.playerEntity.gridCoordinate.value + direction)){
      _gameContext.playerEntity.ReplaceGridCoordinate(_gameContext.playerEntity.gridCoordinate.value + direction);
    }
  }

  private bool IsValidMove(Dictionary<Vector2Int, TileType> map, Vector2Int coordinate){
    return map.ContainsKey(coordinate) && map[coordinate] == TileType.Floor;
  }

  protected override bool Filter(InputEntity entity)
  {
    return entity.hasCommand && _gameContext.isPlayer;
  }

  protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
  {
    return context.CreateCollector(InputMatcher.Command);
  }
}