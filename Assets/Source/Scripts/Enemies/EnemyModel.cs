using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique, CreateAssetMenu(fileName = "EnemyModel", menuName = "Databases/EnemyModel", order = 4)]
public class EnemyModel : ScriptableObject {
	public GameObject prefab;
}