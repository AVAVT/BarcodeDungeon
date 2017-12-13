using Entitas;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TKLibs;

public class MapGeneratorSystem : IInitializeSystem
{
  GameContext _context;
  DungeonSettingModels _settingModels;
  public MapGeneratorSystem(Contexts contexts)
  {
    _context = contexts.game;
    _settingModels = _context.dungeonSettingModels.value;
  }

  public void Initialize()
  {
    // TODO create procedural seed
    CreateDungeonWithSeed((int)System.DateTime.Now.Ticks);
    // CreateDungeonWithSeed(6);
  }

  private void CreateDungeonWithSeed(int seed)
  {
    Dictionary<Vector2Int, TileType> map = GenerateMap(_settingModels.sizes, seed);

    IEnumerable<int> xCoords = Enumerable.Range(0, _settingModels.sizes.x * 2 - 1);
    IEnumerable<int> yCoords = Enumerable.Range(0, _settingModels.sizes.y * 2 - 1);
    foreach (int x in xCoords)
    {
      foreach (int y in yCoords)
      {
        Vector2Int coord = new Vector2Int(x, y);
        GameEntity tile = _context.CreateEntity();
        tile.AddPosition(coord);

        if (map.ContainsKey(coord))
        {
          tile.AddTile(map[coord]);
        }
        else
        {
          tile.AddTile(TileType.Empty);
        }
      }
    }
  }

  private Dictionary<Vector2Int, TileType> GenerateMap(Vector2Int mapSizes, int seed)
  {
    System.Random rng = new System.Random(seed);

    List<BoundsInt> rooms = GenerateRooms(mapSizes, rng);

    List<Vector2Int> visitedNodes = new List<Vector2Int>();
    Dictionary<Vector2Int, TileType> floors = new Dictionary<Vector2Int, TileType>();

    rooms.ForEach(room =>
    {
      foreach (int x in Enumerable.Range(room.x, room.size.x + 1))
      {
        foreach (int y in Enumerable.Range(room.y, room.size.y + 1))
        {
          visitedNodes.Add(new Vector2Int(x, y));
        }
      }

      foreach (int x in Enumerable.Range(room.x * 2, room.size.x * 2 + 1))
      {
        foreach (int y in Enumerable.Range(room.y * 2, room.size.y * 2 + 1))
        {
          floors.Add(new Vector2Int(x, y), TileType.Floor);
        }
      }
    });

    List<int> slots = Enumerable.Range(0, mapSizes.x * mapSizes.y).ToList()
      .FindAll(
        slot => !visitedNodes.Contains(new Vector2Int(slot % mapSizes.x, slot / mapSizes.x))
      );

    int startSlot = slots.RandomItem(rng);
    Vector2Int startNode = new Vector2Int(startSlot % mapSizes.x, startSlot / mapSizes.x);
    floors.Add(startNode * 2, TileType.Floor);

    floors = RecursiveMazeGen(
      new List<Vector2Int> { startNode },
      visitedNodes,
      floors,
      mapSizes,
      rng
    );

    return floors;
  }

  private List<BoundsInt> GenerateRooms(Vector2Int mapSizes, System.Random rng)
  {
    int maxWidth = mapSizes.x % 2 == 1 ? mapSizes.x / 2 : mapSizes.x / 2 - 1;
    int maxHeight = mapSizes.y % 2 == 1 ? mapSizes.y / 2 : mapSizes.y / 2 - 1;

    List<int> slots = Enumerable.Range(0, (mapSizes.x - 1) * (mapSizes.y - 1)).ToList();

    List<BoundsInt> rooms = Enumerable.Range(0, rng.Next(8, 15)).Select(i =>
    {
      int slot = slots.RemoveRandom(rng);
      return new BoundsInt(
        slot % (mapSizes.x - 1),
        slot / (mapSizes.x - 1),
        0,
        0,
        0,
        0
      );
    }).ToList();

    bool allowContact = true; // allow contacting rooms only once
    for (int i = rooms.Count - 1; i >= 0; i--)
    {
      BoundsInt room = rooms[i];
      int maxPotential = GetRoomMaxPotential(mapSizes, rooms, room, 1);
      if (!allowContact) maxPotential -= 1;

      if (maxPotential < 1) rooms.Remove(room);
      else
      {
        int sizeX = rng.Next(1, Mathf.Min(maxWidth, maxPotential));
        int sizeY = rng.Next(1, Mathf.Min(maxHeight, maxPotential));

        // TODO not guaranteed contact here
        if (sizeX == maxPotential || sizeY == maxPotential) allowContact = false;

        rooms[i] = new BoundsInt(
          room.position,
          new Vector3Int(sizeX, sizeY, 0)
        );
      }
    };

    return rooms;
  }

  private int GetRoomMaxPotential(Vector2Int mapSizes, List<BoundsInt> rooms, BoundsInt current, int size)
  {
    BoundsInt test = new BoundsInt(
      current.position,
      Vector3Int.one.WithZ(0) * size
    );

    if (test.max.x <= mapSizes.x && test.max.y <= mapSizes.y && rooms.All(room => room == current || !room.Intersect(test)))
    {
      return GetRoomMaxPotential(mapSizes, rooms, current, size + 1);
    }
    else
    {
      return size - 1;
    }
  }

  private Dictionary<Vector2Int, TileType> RecursiveMazeGen(List<Vector2Int> stack, List<Vector2Int> visitedNodes, Dictionary<Vector2Int, TileType> generated, Vector2Int mapSizes, System.Random rng)
  {
    if (stack.Count == 0) return generated;

    Vector2Int currentNode = stack[stack.Count - 1];
    visitedNodes.Add(currentNode);
    List<Vector2Int> neighbours = GetUnvisitedNeighbours(currentNode, visitedNodes, _settingModels.sizes);

    if (neighbours.Count > 0)
    {
      Vector2Int next = neighbours.RandomItem(rng);

      stack.Add(next);
      visitedNodes.Add(next);
      generated.Add(next * 2, TileType.Floor);
      generated.Add(currentNode + next, TileType.Floor);
    }
    else
    {
      stack.Remove(currentNode);
    }

    return RecursiveMazeGen(
      stack,
      visitedNodes,
      generated,
      mapSizes,
      rng
    );
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