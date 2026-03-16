using UnityEngine;
using UnityEngine.LightTransport;

public class CarnivoreStats : MonoBehaviour
{
    [Header("Eating Settings")][SerializeField] private float timer = 0f, hungerSpeed = 3f, hungerAmount = 5f;
    public HexWorldGenerator world;

    [Range(0f, 100f)] public float hunger = 100, life = 100f;

    public int x;
    public int z;
    public int homeX;
    public int homeZ;

    [Header("Movement Settings")]
    public float runSpeed = 6f;
    public float walkspeed = 3f;
    public float moveSpeed = 3f;
    public float turnSpeed = 10f;

    [Header("Wander Settings")]
    public float waitTime = 1.5f;
    public int wanderRadius = 4;

    [Header("Hunt Settings")]
    public float HuntRadius = 5f;


    [Header("Hunger Damage Settings")]
    public float hungerDamageAmount = 5f, hungerDamageInterval = 15f;

    public bool isEating = false;

    private void Start()
    {
        hunger = Random.Range(0, 60);

        HexCell start = FindClosestCell();
        if (start != null)
        {
            x = start.x;
            z = start.z;

            homeX = start.x;
            homeZ = start.z;

        }
    }
    private void Update()
    {
        if (!isEating)
        {
            Hunger();
        }
        if (hunger <= 0)
        {
            isEating = false;
        }
    }

    private void Hunger()
    {

        timer += Time.deltaTime;
        if (timer > hungerSpeed && hunger < 100)
        {
            hunger += hungerAmount;
            timer = 0f;
        }

        else if (hunger >= 100)
        {
            if (timer > hungerDamageInterval)
            {
                life -= hungerDamageAmount;
                timer = 0f;
            }

        }

    }



    public HexCell FindClosestCell()
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


    public Agent FindClosestDeer()
    {
        Agent[] allAnimals = FindObjectsByType<Agent>(FindObjectsSortMode.None);
        Agent closestDeer = null;
        float bestDistance = Mathf.Infinity;

        foreach (Agent animal in allAnimals)
        {
            HerbivoreStats deer = animal.GetComponent<HerbivoreStats>();

            if (deer == null)
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, animal.transform.position);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                closestDeer = animal;
            }
        }

        return closestDeer;
    }
}
