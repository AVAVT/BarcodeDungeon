using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BreadthFirst
{

    public static PathNode GetRoute(Vector2Int startPos, Vector2Int targetPos, Dictionary<Vector2Int, TileType> map, int maxPathLength)
    {
        List<Vector2Int> visitedNode = new List<Vector2Int>();
        Queue<PathNode> frontier = new Queue<PathNode>();

        visitedNode.Add(startPos);

        PathNode currentNode = new PathNode();
        currentNode.position = startPos;
        currentNode.weight = 0;
        frontier.Enqueue(currentNode);

        while (frontier.Count > 0)
        {
            currentNode = frontier.Dequeue();
            List<Vector2Int> neighbours = GetNeighbourNode(currentNode.position, visitedNode, map);
            foreach (Vector2Int nextPos in neighbours)
            {
                PathNode nextNode = new PathNode();
                nextNode.position = nextPos;
                nextNode.weight = currentNode.weight + 1;
                nextNode.cameFrom = currentNode.position == startPos ? null : currentNode;

                if(nextNode.position == targetPos) return nextNode;

                if(nextNode.weight != maxPathLength && !visitedNode.Contains(nextPos))
                {
                    frontier.Enqueue(nextNode);
                    visitedNode.Add(nextNode.position);
                }
            }
        }

        return null;
    }

    public static List<Vector2Int> GetNeighbourNode(Vector2Int node, List<Vector2Int> visitedNodes, Dictionary<Vector2Int, TileType> map)
    {
        return new List<Vector2Int>{
            node + Vector2Int.up,
            node + Vector2Int.down,
            node + Vector2Int.left,
            node + Vector2Int.right
        }.FindAll(candidate => !visitedNodes.Contains(candidate) && map.ContainsKey(candidate) && map[candidate] == TileType.Floor);
    }
}
public class PathNode {
    public int weight;
    public Vector2Int position;
    public PathNode cameFrom;

    public Vector2Int GetFirstStep(){
        return cameFrom == null ? position : cameFrom.GetFirstStep();
    }
}