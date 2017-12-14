using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique, CreateAssetMenu(fileName = "PlayerModel", menuName = "Databases/PlayerModel", order = 2)]
public class PlayerModel : ScriptableObject {
  public GameObject prefab;
}