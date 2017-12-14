using Entitas;
using UnityEngine;
using Lean.Touch;

public class ControllerSystem : IInitializeSystem
{
  InputContext _context;
  public ControllerSystem(Contexts contexts)
  {
    _context = contexts.input;
  }
  public void Initialize()
  {
    Lean.Touch.LeanTouch.OnFingerSwipe += OnFingerSwipe;
  }

  void OnFingerSwipe(LeanFinger finger)
  {
    if (finger.SwipeScreenDelta.magnitude > 10)
    {
      if (Mathf.Abs(finger.SwipeScreenDelta.x) > Mathf.Abs(finger.SwipeScreenDelta.y))
      {
        _context.ReplaceCommand(Vector2Int.right * (int)Mathf.Sign(finger.SwipeScreenDelta.x));
      }
      else
      {
        _context.ReplaceCommand(Vector2Int.up * (int)Mathf.Sign(finger.SwipeScreenDelta.y));
      }
    }
  }
}