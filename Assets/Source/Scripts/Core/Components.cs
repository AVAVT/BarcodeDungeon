using Entitas;
using UnityEngine;

public class GridCoordinateComponent : IComponent{
  public Vector2Int value;
}

public class PositionComponent : IComponent{
  public Vector2 value;
}

public class SpriteComponent : IComponent {
  public Sprite value;
}

public class ViewComponent : IComponent {
  public GameObject gameObject;
}
