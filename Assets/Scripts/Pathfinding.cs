using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static List<DataTile> FindPath(DataTile startTile, DataTile endTile)
    {
        List<DataTile> openSet = new List<DataTile>();
        HashSet<DataTile> closedSet = new HashSet<DataTile>();

        Dictionary<DataTile, float> gCost = new Dictionary<DataTile, float>();
        Dictionary<DataTile, float> fCost = new Dictionary<DataTile, float>();
        Dictionary<DataTile, DataTile> cameFrom = new Dictionary<DataTile, DataTile>();

        openSet.Add(startTile);
        gCost[startTile] = 0;
        fCost[startTile] = GetDistance(startTile, endTile);

        while (openSet.Count > 0)
        {
            DataTile current = GetLowestFCostTile(openSet, fCost, gCost);

            if(current == endTile)
            {
                return RetracePath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (DataTile neighbor in current.neighbors)
            {
                if (neighbor == null || !neighbor.terrainTile.walkable || closedSet.Contains(neighbor)) continue;

                float tentativeGCost = gCost[current] + neighbor.terrainTile.pathWeight;

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

        return null;
    }

    static float GetDistance(DataTile tileA, DataTile tileB)
    {
        return Mathf.Abs(tileA.position.x - tileB.position.x) + Mathf.Abs(tileA.position.y - tileB.position.y);
    }

    static DataTile GetLowestFCostTile(List<DataTile> openList, Dictionary<DataTile, float> fCost, Dictionary<DataTile, float> gCost)
    {
        DataTile lowestCostTile = openList[0];
        float lowestFCost = fCost[lowestCostTile];

        foreach (DataTile tile in openList)
        {
            if (fCost[tile] < lowestFCost)
            {
                lowestFCost = fCost[tile];
                lowestCostTile = tile;
            }
        }

        return lowestCostTile;
    }

    static List<DataTile> RetracePath(Dictionary<DataTile, DataTile> cameFrom, DataTile current)
    {
        List<DataTile> path = new List<DataTile>();
        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}
