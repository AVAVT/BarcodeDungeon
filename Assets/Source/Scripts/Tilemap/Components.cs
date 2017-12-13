using Entitas;
using UnityEngine;

public class TileComponent : IComponent{
  public TileType type;
}

public class GridCoordinateComponent : IComponent{
  public Vector2Int value;
}

public enum TileType {
  Empty,
  Floor,
  Wall
}