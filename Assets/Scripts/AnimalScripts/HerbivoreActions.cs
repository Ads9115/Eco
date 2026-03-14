using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.LightTransport;

public class HerbivoreActions : MonoBehaviour
{

    private bool isWaiting = false;
    [SerializeField] float waitTimer = 1f;
    HexCell currentTarget;
    Queue<HexCell> path = new Queue<HexCell>();

    AnimalsStats stats;
    Agent agent;
    HerbivoreAi myBrain;
    public HexWorldGenerator world;

    public enum HerbivoreStates
    {
        Wander,
        SeekFood,
        EatFood,
        FallBak,
        Escape
    }

    public HerbivoreStates currentState = HerbivoreStates.Wander;
    private void Start()
    {
        myBrain=GetComponent<HerbivoreAi>();
        agent = GetComponent<Agent>();
        stats=GetComponent<AnimalsStats>();
        ChooseWanderDestination();
    }

    private void Update()
    {
        if (currentState == HerbivoreStates.Wander)
        {
            Wander();
        }
    }


    private void Wander()
    {
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
        float moveStep =stats.moveSpeed * Time.deltaTime;

        if (distanceToTarget > 0.05f)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * stats.turnSpeed);
            }
        }

        if (distanceToTarget <= moveStep)
        {
            transform.position = targetPosition;

            stats.x = currentTarget.x;
            stats.z = currentTarget.z;

            if (path.Count > 0)
            {
                currentTarget = path.Dequeue();
            }
            else
            {
                StartWaiting(stats.waitTime);
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

    public void ChooseWanderDestination()
    {
        HexCell start = world.GetCell(stats.x, stats.z);

        for (int i = 0; i < 40; i++)
        {
            int rx = stats.homeX + Random.Range(-stats.wanderRadius, stats.wanderRadius + 1);
            int rz = stats.homeZ + Random.Range(-stats.wanderRadius, stats.wanderRadius + 1);

            HexCell goal = world.GetCell(rx, rz);

            if (goal == null || goal.isWater || goal.isTree)
                continue;

            List<HexCell> newPath = agent.AStar(start, goal);

            if (newPath != null && newPath.Count > 0)
            {
                path = new Queue<HexCell>(newPath);
                currentTarget = path.Dequeue();
                return;
            }
        }

        StartWaiting(0.5f);
    }
}
