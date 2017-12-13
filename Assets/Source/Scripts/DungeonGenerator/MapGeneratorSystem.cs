using Entitas;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    // TODO create procedural seed
    Dictionary<Vector2Int, TileType> map = GenerateMap(_settingModels.sizes, (int)System.DateTime.Now.Ticks);

    foreach (KeyValuePair<Vector2Int, TileType> kvp in map)
    {
      GameEntity tile = _context.CreateEntity();
      tile.AddPosition(kvp.Key);
      tile.AddSprite(kvp.Value == TileType.Empty ? _tileModels.blackTile : _tileModels.floor);
    }
  }

  private Dictionary<Vector2Int, TileType> GenerateMap(Vector2Int mapSizes, int seed)
  {
    System.Random rng = new System.Random(seed);

    Dictionary<Vector2Int, TileType> result = new Dictionary<Vector2Int, TileType>();

    IEnumerable<int> xCoords = Enumerable.Range(0, mapSizes.x * 2 - 1);
    IEnumerable<int> yCoords = Enumerable.Range(0, mapSizes.y * 2 - 1);
    foreach(int x in xCoords){
      foreach(int y in yCoords){
        result.Add(new Vector2Int(x, y), TileType.Empty);
      }
    }

    List<Vector2Int> visitedNodes = new List<Vector2Int>();
    List<Vector2Int> stack = new List<Vector2Int>();

    Vector2Int currentNode = new Vector2Int(
      rng.Next(mapSizes.x),
      rng.Next(mapSizes.y)
    );

    stack.Add(currentNode);
    visitedNodes.Add(currentNode);
    result[currentNode * 2] = TileType.Floor;

    while (stack.Count > 0)
    {
      currentNode = stack[stack.Count - 1];
      List<Vector2Int> neighbours = GetUnvisitedNeighbours(currentNode, visitedNodes, _settingModels.sizes);

      if (neighbours.Count > 0)
      {
        Vector2Int next = neighbours.RandomItem(rng);
        stack.Add(next);
        visitedNodes.Add(next);
        result[next * 2] = TileType.Floor;

        result[currentNode * 2 + (next - currentNode)] = TileType.Floor;
      }
      else
      {
        stack.Remove(currentNode);
      }
    }

    return result;
  }

  private List<Vector2Int> GetUnvisitedNeighbours(Vector2Int node, List<Vector2Int> visitedNodes, Vector2Int sizes)
  {
    return new List<Vector2Int>{
      node + Vector2Int.up,
      node + Vector2Int.down,
      node + Vector2Int.left,
      node + Vector2Int.right
    }.FindAll(candidate => IsValidNode(candidate, visitedNodes, sizes));
  }

  private bool IsValidNode(Vector2Int node, List<Vector2Int> visitedNodes, Vector2Int sizes)
  {
    return node.x >= 0 && node.x < sizes.x && node.y >= 0 && node.y < sizes.y && !visitedNodes.Contains(node);
  }
}