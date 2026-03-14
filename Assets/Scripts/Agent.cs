using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public HexWorldGenerator world;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    [SerializeField] float turnSpeed = 10f;

    [Header("Wander Settings")]
    [SerializeField] float waitTime = 1.5f;
    [SerializeField] int wanderRadius = 4;

    public int x;
    public int z;

    private int homeX;
    private int homeZ;

    private float waitTimer = 0f;
    private bool isWaiting = false;

    Queue<HexCell> path = new Queue<HexCell>();
    HexCell currentTarget;

    void Start()
    {
        HexCell start = FindClosestCell();
        if (start != null)
        {
            x = start.x;
            z = start.z;

            homeX = start.x;
            homeZ = start.z;

            ChooseWanderDestination();
        }
    }

    private void MyBrain_OnAnimalHungry(object sender, System.EventArgs e)
    {
        
    }

    void Update()
    {
        // If the agent is waiting, we exit early. NO movement and NO rotation allowed here.
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                ChooseWanderDestination();
            }
            return;
        }

        if (currentTarget == null)
        {
            StartWaiting(0.5f);
            return;
        }

        Vector3 targetPosition = new Vector3(
            currentTarget.transform.position.x,
            transform.position.y,
            currentTarget.transform.position.z
        );

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        float moveStep = moveSpeed * Time.deltaTime;

        if (distanceToTarget > 0.05f)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
            }
        }

        if (distanceToTarget <= moveStep)
        {
            transform.position = targetPosition;

            x = currentTarget.x;
            z = currentTarget.z;

            if (path.Count > 0)
            {
                currentTarget = path.Dequeue();
            }
            else
            {
                StartWaiting(waitTime);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveStep
            );
        }
    }

    void StartWaiting(float timeToWait)
    {
        isWaiting = true;
        waitTimer = timeToWait;
        currentTarget = null;
    }

    HexCell FindClosestCell()
    {
        HexCell closest = null;
        float best = Mathf.Infinity;

        for (int i = 0; i < world.size; i++)
        {
            for (int j = 0; j < world.size; j++)
            {
                HexCell c = world.GetCell(i, j);
                if (c == null) continue;

                float d = Vector3.Distance(transform.position, c.transform.position);
                if (d < best)
                {
                    best = d;
                    closest = c;
                }
            }
        }

        return closest;
    }

    void ChooseWanderDestination()
    {
        HexCell start = world.GetCell(x, z);

        for (int i = 0; i < 40; i++)
        {
            int rx = homeX + Random.Range(-wanderRadius, wanderRadius + 1);
            int rz = homeZ + Random.Range(-wanderRadius, wanderRadius + 1);

            HexCell goal = world.GetCell(rx, rz);

            if (goal == null || goal.isWater || goal.isTree)
                continue;

            List<HexCell> newPath = AStar(start, goal);

            if (newPath != null && newPath.Count > 0)
            {
                path = new Queue<HexCell>(newPath);
                currentTarget = path.Dequeue();
                return;
            }
        }

        StartWaiting(0.5f);
    }


    List<HexCell> AStar(HexCell start, HexCell goal)
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