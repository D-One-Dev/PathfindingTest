using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static List<GameTile> FindPath(GameTile startTile, GameTile endTile)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        List<GameTile> openSet = new List<GameTile>();
        HashSet<GameTile> closedSet = new HashSet<GameTile>();

        Dictionary<GameTile, float> gCost = new Dictionary<GameTile, float>();
        Dictionary<GameTile, float> fCost = new Dictionary<GameTile, float>();
        Dictionary<GameTile, GameTile> cameFrom = new Dictionary<GameTile, GameTile>();

        openSet.Add(startTile);
        gCost[startTile] = 0;
        fCost[startTile] = GetDistance(startTile, endTile);

        while (openSet.Count > 0)
        {
            GameTile current = GetLowestFCostTile(openSet, fCost, gCost);

            if(current == endTile)
            {
                watch.Stop();
                Debug.LogWarningFormat("Pathfinding time: " + watch.ElapsedMilliseconds + " msec.");

                return RetracePath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (GameTile neighbor in current.neighbors)
            {
                if (neighbor == null || neighbor.surfaceType == SurfaceType.Water || closedSet.Contains(neighbor)) continue;

                float tentativeGCost = gCost[current] + neighbor.pathWeight;

                if (!openSet.Contains(neighbor))
                { 
                    openSet.Add(neighbor);
                }
                else if (tentativeGCost >= gCost[neighbor]) continue;

                cameFrom[neighbor] = current;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + GetDistance(neighbor, endTile);
            }
        }
        watch.Stop();
        Debug.LogWarningFormat("Pathfinding time: " + watch.ElapsedMilliseconds + " msec.");

        return null;
    }

    static float GetDistance(GameTile tileA, GameTile tileB)
    {
        return Mathf.Abs(tileA.position.x - tileB.position.x) + Mathf.Abs(tileA.position.y - tileB.position.y);
    }

    static GameTile GetLowestFCostTile(List<GameTile> openList, Dictionary<GameTile, float> fCost, Dictionary<GameTile, float> gCost)
    {
        GameTile lowestCostTile = openList[0];
        float lowestFCost = fCost[lowestCostTile];

        foreach (GameTile tile in openList)
        {
            if (fCost[tile] < lowestFCost)
            {
                lowestFCost = fCost[tile];
                lowestCostTile = tile;
            }
        }

        return lowestCostTile;
    }

    static List<GameTile> RetracePath(Dictionary<GameTile, GameTile> cameFrom, GameTile current)
    {
        List<GameTile> path = new List<GameTile>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}
