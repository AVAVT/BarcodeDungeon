using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using TKLibs;
using System.Linq;

public class EnemySpawnSystem : ReactiveSystem<GameEntity>, IInitializeSystem
{
	GameContext _context;
	IGroup<GameEntity> generatorGroup;

	public EnemySpawnSystem (Contexts contexts) : base (contexts.game)
	{
		_context = contexts.game;
	}

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		generatorGroup = _context.GetGroup (GameMatcher.Generator);
	}

	#endregion

	protected override ICollector<GameEntity> GetTrigger (IContext<GameEntity> context)
	{
		return context.CreateCollector (GameMatcher.Dungeon.Added ());
	}

	protected override bool Filter (GameEntity entity)
	{
		return entity.hasDungeon;
	}

	protected override void Execute (List<GameEntity> entities)
	{
		int seed = generatorGroup.GetSingleEntity ().generator.seed;
		System.Random rng = new System.Random (seed);
		int enemiesNumber = rng.Next (10,15);

		DungeonComponent dungeon = entities.SingleEntity ().dungeon;
		for (int i = 0; i < enemiesNumber; i++) {			
			GameEntity enemyEntity = _context.CreateEntity ();
			GameObject enemyObject = GameObject.Instantiate (_context.enemyModel.value.prefab);
			enemyEntity.AddView (enemyObject);
			enemyObject.Link (enemyEntity, _context);
			enemyEntity.isMoveable = true;
			rng = new System.Random (seed+i);
			Vector2Int spawnLocation = RandomSpawnPosition (dungeon.map,rng);
			enemyEntity.ReplacePosition (spawnLocation);
			enemyEntity.ReplaceGridCoordinate (spawnLocation);
		}

	}

	public Vector2Int RandomSpawnPosition (Dictionary<Vector2Int, TileType> map, System.Random rng)
	{
		Dictionary<Vector2Int, TileType> filterDic = map.Where (items => items.Value == TileType.Floor).ToDictionary (items => items.Key, items => items.Value);
		List<Vector2Int> location = new List<Vector2Int> ();
		foreach (KeyValuePair<Vector2Int, TileType> slot in filterDic) {
			location.Add (slot.Key);
		}
		Vector2Int coordinate = location.RandomItem (rng);
		if (coordinate == _context.dungeon.entry || coordinate == _context.dungeon.exit) {
			rng = new System.Random (generatorGroup.GetSingleEntity ().generator.seed+200);
			coordinate = RandomSpawnPosition (map, rng);
		}
		Debug.Log (coordinate);
		return coordinate;
	}
		
}