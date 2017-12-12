using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game, Unique, CreateAssetMenu(fileName = "TileModels", menuName = "Databases/TileModels", order = 1)]
public class TileModels : ScriptableObject {
  public Sprite wallTopLeft;
  public Sprite wallTop;
  public Sprite wallTopRight;
  public Sprite wallCenter;
  public Sprite wallTopT;
  public Sprite wallVertical;
  public Sprite wallPyramid;
  public Sprite wallLeftT;
  public Sprite wallCross;
  public Sprite wallRightT;
  public Sprite wallBottomLeft;
  public Sprite wallBottomRight;
  public Sprite wallBottomT;
  public Sprite blackTile;
  public Sprite floor;
}