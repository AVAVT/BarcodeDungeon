using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;


[Game, Unique]
public class TurnComponent : IComponent { 
	public TurnType type;
}

public enum TurnType {
	Player,
	Enemy,
}
