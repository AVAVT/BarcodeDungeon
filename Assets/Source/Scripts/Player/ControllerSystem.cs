using Entitas;
using UnityEngine;


public class ControllerSystem : IExecuteSystem
{
  InputContext _context;
  public ControllerSystem(Contexts contexts){
    _context = contexts.input;
  }

  public void Execute()
  {
    if(Input.GetKeyDown(KeyCode.W)){
      _context.ReplaceCommand(Vector2Int.up);
    }
    else if(Input.GetKeyDown(KeyCode.A)){
      _context.ReplaceCommand(Vector2Int.left);
    }
    else if(Input.GetKeyDown(KeyCode.S)){
      _context.ReplaceCommand(Vector2Int.down);
    }
    else if(Input.GetKeyDown(KeyCode.D)){
      _context.ReplaceCommand(Vector2Int.right);
    }
  }
}