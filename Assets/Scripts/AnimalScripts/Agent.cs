using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
  public List<HexCell> AStar(HexCell start, HexCell goal)
    {
        List<HexCell> open = new List<HexCell>();
        HashSet<HexCell> closed = new HashSet<HexCell>();

        Dictionary<HexCell, HexCell> cameFrom = new Dictionary<HexCell, HexCell>();
        Dictionary<HexCell, float> gScore = new Dictionary<HexCell, float>();
        Dictionary<HexCell, float> fScore = new Dictionary<HexCell, float>();

        open.Add(start);
        gScore[start] = 0;
        fScore[start] = Vector3.Distance(start.transform.position, goal.transform.position);

        while (open.Count > 0)
        {
            HexCell current = open[0];

            foreach (HexCell c in open)
            {
                if (fScore.ContainsKey(c) && fScore[c] < fScore[current])
                    current = c;
            }

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            open.Remove(current);
            closed.Add(current);

            foreach (HexCell n in current.neighbors)
            {
                if (n == null || n.isWater || n.isTree || closed.Contains(n))
                    continue;

                float tentativeGScore = gScore[current] + 1;

                if (!gScore.ContainsKey(n) || tentativeGScore < gScore[n])
                {
                    cameFrom[n] = current;
                    gScore[n] = tentativeGScore;
                    fScore[n] = tentativeGScore + Vector3.Distance(n.transform.position, goal.transform.position);

                    if (!open.Contains(n))
                        open.Add(n);
                }
            }
        }

        return null;
    }

    List<HexCell> ReconstructPath(Dictionary<HexCell, HexCell> cameFrom, HexCell current)
    {
        List<HexCell> path = new List<HexCell>();

        while (cameFrom.ContainsKey(current))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Reverse();
        return path;
    }
}