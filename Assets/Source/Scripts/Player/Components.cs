using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;


[Game, Unique]
public class PlayerComponent : IComponent { }

[Input, Unique]
public class CommandComponent : IComponent {
  public Vector2Int direction;
}