using Entitas;
using UnityEngine;

public class PositionComponent : IComponent{
  public Vector2 value;
}

public class SpriteComponent : IComponent {
  public Sprite value;
}

public class ViewComponent : IComponent {
  public GameObject gameObject;
}
