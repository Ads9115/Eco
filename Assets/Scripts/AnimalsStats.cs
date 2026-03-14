using UnityEngine;

public class AnimalsStats : MonoBehaviour
{

    public float hunger = 100;

    public float speed = 5f;

    private void Start()
    {
        hunger = Random.Range(0, 60);
    }

    
}
