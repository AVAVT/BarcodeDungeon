using Entitas;
using UnityEngine;
using System.Collections.Generic;

public class MapGeneratorSystem : IInitializeSystem
{
  GameContext _context;
  DungeonSettingModels _settingModels;
  TileModels _tileModels;
  public MapGeneratorSystem(Contexts contexts)
  {
    _context = contexts.game;
    _settingModels = _context.dungeonSettingModels.value;
    _tileModels = _context.tileModels.value;
  }

  public void Initialize()
  {
    Dictionary<Vector2Int, TileType> map = new Dictionary<Vector2Int, TileType>();

    for (int x = 0; x < _settingModels.sizes.x * 2 - 1; x++)
    {
      for (int y = 0; y < _settingModels.sizes.y * 2 - 1; y++)
      {
        map.Add(new Vector2Int(x, y), TileType.Empty);
      }
    }

    List<Vector2Int> visitedNodes = new List<Vector2Int>();
    List<Vector2Int> stack = new List<Vector2Int>();

    Vector2Int currentNode = new Vector2Int(
      Random.Range(0, _settingModels.sizes.x),
      Random.Range(0, _settingModels.sizes.y)
    );

    stack.Add(currentNode);
    visitedNodes.Add(currentNode);
    map[currentNode * 2] = TileType.Floor;

    while (stack.Count > 0)
    {
      currentNode = stack[stack.Count - 1];
      List<Vector2Int> neighbours = GetUnvisitedNeighbours(currentNode, visitedNodes, _settingModels.sizes);

      if (neighbours.Count > 0)
      {
        Vector2Int next = neighbours.RandomItem();
        stack.Add(next);
        visitedNodes.Add(next);
        map[next * 2] = TileType.Floor;

        map[currentNode * 2 + (next - currentNode)] = TileType.Floor;
      }
      else
      {
        stack.Remove(currentNode);
      }
    }

    foreach (KeyValuePair<Vector2Int, TileType> kvp in map)
    {
      GameEntity tile = _context.CreateEntity();
      tile.AddPosition(kvp.Key);
      tile.AddSprite(kvp.Value == TileType.Empty ? _tileModels.blackTile : _tileModels.floor);
    }
  }

  private List<Vector2Int> GetUnvisitedNeighbours(Vector2Int node, List<Vector2Int> visitedNodes, Vector2Int sizes)
  {
    List<Vector2Int> result = new List<Vector2Int>();

    Vector2Int candidate = node + Vector2Int.up;
    if (IsValidNode(candidate, visitedNodes, sizes)) result.Add(candidate);

    candidate = node + Vector2Int.down;
    if (IsValidNode(candidate, visitedNodes, sizes)) result.Add(candidate);

    candidate = node + Vector2Int.left;
    if (IsValidNode(candidate, visitedNodes, sizes)) result.Add(candidate);

    candidate = node + Vector2Int.right;
    if (IsValidNode(candidate, visitedNodes, sizes)) result.Add(candidate);

    return result;
  }

  private bool IsValidNode(Vector2Int node, List<Vector2Int> visitedNodes, Vector2Int sizes)
  {
    return node.x >= 0 && node.x < sizes.x && node.y >= 0 && node.y < sizes.y && !visitedNodes.Contains(node);
  }
}