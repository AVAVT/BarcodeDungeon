using Entitas;
using UnityEngine;

public class TileComponent : IComponent{
  public TileType type;
}

public enum TileType {
  Empty,
  Floor
}