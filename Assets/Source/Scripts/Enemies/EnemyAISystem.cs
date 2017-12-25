using System.Collections.Generic;
using Entitas;
using UnityEngine;
using System.Linq;
public class EnemyAISystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
    GameContext _context;

    IGroup<GameEntity> enemyGroup;
    public EnemyAISystem(Contexts contexts) : base(contexts.game)
    {
        _context = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        MoveEnemy(entities, _context.playerEntity, _context.dungeon.map);
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isEnemy && !entity.isMoveCompleted;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector(GameMatcher.MoveCompleted.Removed());
    }

    void IInitializeSystem.Initialize()
    {
        enemyGroup = _context.GetGroup(GameMatcher.Enemy);
    }

    private void MoveEnemy(List<GameEntity> enemies, GameEntity player, Dictionary<Vector2Int, TileType> map)
    {
        enemies.ForEach(enemy => {
            PathNode result = BreadthFirst.GetRoute(enemy.gridCoordinate.value, player.gridCoordinate.value, map, 4);
            if (result != null)
            {
                enemy.ReplacePosition(result.GetFirstStep());
                enemy.ReplaceGridCoordinate(result.GetFirstStep());
            }
            else
            {
                Vector2Int randomPos = new List<Vector2Int>{
                    enemy.gridCoordinate.value + Vector2Int.up,
                    enemy.gridCoordinate.value + Vector2Int.down,
                    enemy.gridCoordinate.value + Vector2Int.left,
                    enemy.gridCoordinate.value + Vector2Int.right
                }.FindAll(pos => IsFloorTile(pos, map)).RandomItem();
                enemy.ReplacePosition(randomPos);
                enemy.ReplaceGridCoordinate(randomPos);
            }
            enemy.isMoveCompleted = true;
        });
    }

    private bool IsFloorTile(Vector2Int pos, Dictionary<Vector2Int, TileType> map){
        return map.ContainsKey(pos) && map[pos] == TileType.Floor;
    }
}

