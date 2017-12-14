using Entitas;
using UnityEngine;

[Game]
public class TileComponent : IComponent{
  public TileType type;
}

[Game]
public class GridCoordinateComponent : IComponent{
  public Vector2Int value;
}

public enum TileType {
  Empty,
  Floor,
  Wall
}