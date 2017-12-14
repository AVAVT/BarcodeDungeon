using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using System.Collections.Generic;

[Game, Unique]
public class GeneratorComponent : IComponent {
  public int seed;
}

[Game, Unique]
public class DungeonComponent : IComponent {
  public Dictionary<Vector2Int, TileType> map;
  public Vector2Int entry;
  public Vector2Int exit;
}