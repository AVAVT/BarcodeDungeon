using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique, CreateAssetMenu(fileName = "DungeonSettings", menuName = "Settings/DungeonSettings", order = 1)]
public class DungeonSettingModels : ScriptableObject {
  public Vector2Int sizes;
}