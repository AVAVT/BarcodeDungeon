using Entitas;

public class EnemySystems : Feature {
	public EnemySystems(Contexts contexts): base("Enemy System"){
		Add (new EnemySpawnSystem (contexts));
	}
}
