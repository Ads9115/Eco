using UnityEngine;
using UnityEngine.LightTransport;

public class AnimalsStats : MonoBehaviour
{

    public HexWorldGenerator world;

    public float hunger = 100;

    public int x;
    public int z;
    public int homeX;
    public int homeZ;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float turnSpeed = 10f;

    [Header("Wander Settings")]
    public float waitTime = 1.5f;
    public int wanderRadius = 4;


    public bool isEating=false;

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

}
