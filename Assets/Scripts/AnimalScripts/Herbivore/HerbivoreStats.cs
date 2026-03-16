using UnityEngine;
using UnityEngine.LightTransport;

public class HerbivoreStats : MonoBehaviour
{
    [Header("Eating Settings")][SerializeField]private float timer = 0f, hungerSpeed = 3f, hungerAmount = 5f;
    public HexWorldGenerator world;

    [Range(0f,100f)]public float hunger = 100,life=100f;

    public int x;
    public int z;
    public int homeX;
    public int homeZ;

    [Header("Movement Settings")]
    public float walkSpeed = 3f,runSpeed=5f;
    public float moveSpeed = 3f;
    public float turnSpeed = 10f;

    [Header("Wander Settings")]
    public float waitTime = 1.5f;
    public int wanderRadius = 4;


    [Header("Hunger Damage Settings")]
    public float hungerDamageAmount = 5f, hungerDamageInterval = 15f;

    public bool isEating = false;

    private void Start()
    {
        moveSpeed = walkSpeed;
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
        if (timer > hungerSpeed&&hunger<100)
        {
            hunger+=hungerAmount;  
            timer = 0f;
        }
        
        else if (hunger >= 100)
        {
            if (timer > hungerDamageInterval)
            {
                life-=hungerDamageAmount;
                timer = 0f;
            }
            
        }
        
    }

    [Header("Escape Settings")]
    public float fleeRadius = 10f;

    public Agent FindClosestTiger()
    {
        Agent[] allAnimals = FindObjectsByType<Agent>(FindObjectsSortMode.None);
        Agent closestTiger = null;
        float bestDistance = Mathf.Infinity;

        foreach (Agent animal in allAnimals)
        {
            CarnivoreStats tiger = animal.GetComponent<CarnivoreStats>();
            if (tiger == null) continue;

            float distance = Vector3.Distance(transform.position, animal.transform.position);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                closestTiger = animal;
            }
        }
        return closestTiger;
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

    public HexCell FindClosestCell(string cellType)
    {
        if (cellType == "GrassHex")
        {
            HexCell closest = null;

            float best = Mathf.Infinity;
            for (int x = 0; x < world.size; x++)
            {
                for (int z = 0; z < world.size; z++)
                {
                    HexCell cell = world.GetCell(x, z);

                   
                    if (cell == null||!cell.isGrass) continue;

                    GrassManager grass = cell.GetComponent<GrassManager>();
                    if (grass == null|| grass.grassAmount < 20) continue;

                    
                    float d = Vector3.Distance(transform.position, cell.transform.position);
                    if (d < best)
                    {
                        best = d;
                        closest = cell;
                    }
                }
            }
            return closest;
        }
        else { return null;}
        
    }


}
