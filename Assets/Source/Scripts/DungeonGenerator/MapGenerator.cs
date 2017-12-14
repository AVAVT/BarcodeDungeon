using UnityEngine;
using System.Collections.Generic;
using TKLibs;
using System.Linq;

public class MapGenerator {
  public static Dictionary<Vector2Int, TileType> GenerateMap(Vector2Int mapSizes, System.Random rng)
  {
    List<BoundsInt> rooms = GenerateRooms(mapSizes, rng);

    List<Vector2Int> visitedNodes = new List<Vector2Int>();
    Dictionary<Vector2Int, TileType> floors = new Dictionary<Vector2Int, TileType>();

    List<Vector2Int> openedDoors = new List<Vector2Int>();
    rooms.ForEach(room =>
    {
      visitedNodes.AddRange(room.All2DPoints());

      BoundsInt roomOnMap = new BoundsInt(
        room.position * 2,
        room.size * 2
      );
      roomOnMap.All2DPoints().ForEach(point => {
        floors.Add(point, TileType.Floor);
      });

      // open door
      List<Vector2Int> doorCandidates = GetPossibleDoorForRoom(room, mapSizes);
      Vector2Int door = doorCandidates.FindAll(item => !openedDoors.Contains(item)).RandomItem(rng);
      floors.Add(door, TileType.Floor);
      openedDoors.Add(door);
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

  private static List<Vector2Int> GetPossibleDoorForRoom(BoundsInt room, Vector2Int mapSizes){
    List<Vector2Int> result = new List<Vector2Int>();
    for(int x=room.min.x; x <= room.max.x; x++){
      if(room.min.y > 0){
        result.Add(new Vector2Int(x*2, room.min.y*2 - 1));
      }
      if(room.max.y < mapSizes.y - 1){
        result.Add(new Vector2Int(x*2, room.max.y*2 + 1));
      }
    }

    for(int y=room.min.y; y <= room.max.y; y++){
      if(room.min.x > 0){
        result.Add(new Vector2Int(room.min.x*2 - 1, y*2));
      }
      if(room.max.x < mapSizes.x - 1){
        result.Add(new Vector2Int(room.max.x*2 + 1, y*2));
      }
    }

    return result;
  }

  private static List<BoundsInt> GenerateRooms(Vector2Int mapSizes, System.Random rng)
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

  private static int GetRoomMaxPotential(Vector2Int mapSizes, List<BoundsInt> rooms, BoundsInt current, int size)
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

  private static Dictionary<Vector2Int, TileType> RecursiveMazeGen(List<Vector2Int> stack, List<Vector2Int> visitedNodes, Dictionary<Vector2Int, TileType> generated, Vector2Int mapSizes, System.Random rng)
  {
    if (stack.Count == 0) return generated;

    Vector2Int currentNode = stack[stack.Count - 1];
    visitedNodes.Add(currentNode);
    List<Vector2Int> neighbours = GetUnvisitedNeighbours(currentNode, visitedNodes, mapSizes);

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

  private static List<Vector2Int> GetUnvisitedNeighbours(Vector2Int node, List<Vector2Int> visitedNodes, Vector2Int sizes)
  {
    return new List<Vector2Int>{
      node + Vector2Int.up,
      node + Vector2Int.down,
      node + Vector2Int.left,
      node + Vector2Int.right
    }.FindAll(candidate => IsValidNode(candidate, visitedNodes, sizes));
  }

  private static bool IsValidNode(Vector2Int node, List<Vector2Int> visitedNodes, Vector2Int sizes)
  {
    return node.x >= 0 && node.x < sizes.x && node.y >= 0 && node.y < sizes.y && !visitedNodes.Contains(node);
  }
}