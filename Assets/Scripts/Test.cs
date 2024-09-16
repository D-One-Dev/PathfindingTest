/**using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Test
{
    public static List<DataTile> FindPath(DataTile startPoint, DataTile endPoint)
    {
        List<DataTile> openPathTiles = new List<DataTile>();
        List<DataTile> closedPathTiles = new List<DataTile>();

        // Prepare the start tile.
        DataTile currentTile = startPoint;

        currentTile.SetG(0);
        currentTile.SetH(GetEstimatedPathCost(startPoint.cubicPosition, endPoint.cubicPosition));

        // Add the start tile to the open list.
        openPathTiles.Add(currentTile);

        while (openPathTiles.Count != 0)
        {
            // Sorting the open list to get the tile with the lowest F.
            openPathTiles = openPathTiles.OrderBy(x => x.F).ThenByDescending(x => x.G).ToList();
            currentTile = openPathTiles[0];

            // Removing the current tile from the open list and adding it to the closed list.
            openPathTiles.Remove(currentTile);
            closedPathTiles.Add(currentTile);

            int g = currentTile.G + 1;

            // If there is a target tile in the closed list, we have found a path.
            if (closedPathTiles.Contains(endPoint))
            {
                break;
            }

            // Investigating each adjacent tile of the current tile.
            foreach (DataTile adjacentTile in DataTilemap.Instance.GetNeighbourTiles(currentTile))
            {
                // Ignore not walkable adjacent tiles.
                if (!adjacentTile.terrainTile.walkable)
                {
                    continue;
                }

                // Ignore the tile if it's already in the closed list.
                if (closedPathTiles.Contains(adjacentTile))
                {
                    continue;
                }

                // If it's not in the open list - add it and compute G and H.
                if (!(openPathTiles.Contains(adjacentTile)))
                {
                    adjacentTile.SetG(g);
                    adjacentTile.SetH(GetEstimatedPathCost(adjacentTile.cubicPosition, endPoint.cubicPosition));
                    openPathTiles.Add(adjacentTile);
                }
                // Otherwise check if using current G we can get a lower value of F, if so update it's value.
                else if (adjacentTile.F > g + adjacentTile.H)
                {
                    adjacentTile.SetG(g);
                }
            }
        }

        List<DataTile> finalPathTiles = new List<DataTile>();

        // Backtracking - setting the final path.
        if (closedPathTiles.Contains(endPoint))
        {
            currentTile = endPoint;
            finalPathTiles.Add(currentTile);

            for (int i = endPoint.G - 1; i >= 0; i--)
            {
                currentTile = closedPathTiles.Find(x => x.G == i && DataTilemap.Instance.GetNeighbourTiles(currentTile).Contains(x));
                finalPathTiles.Add(currentTile);
            }

            finalPathTiles.Reverse();
        }

        return finalPathTiles;
    }
    protected static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition)
    {
        return Mathf.Max(Mathf.Abs(startPosition.z - targetPosition.z), Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x), Mathf.Abs(startPosition.y - targetPosition.y)));
    }
}**/