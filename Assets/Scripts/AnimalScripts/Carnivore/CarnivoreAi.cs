using UnityEngine;

public class CarnivoreAi : MonoBehaviour
{
    AnimalCount animalCount;
    private Agent agent;
    private CarnivoreStats stats;
    private CarnivoreActions actions;


    private void Start()
    {
        animalCount = FindAnyObjectByType<AnimalCount>();
        actions = GetComponent<CarnivoreActions>();
        agent = GetComponent<Agent>();
        stats = GetComponent<CarnivoreStats>();
    }

    private void Update()
    {

        if (stats.hunger > 60 && !stats.isEating && actions.currentState != CarnivoreActions.CarnivoreStates.SeekFood&&actions.currentState!=CarnivoreActions.CarnivoreStates.Hunt)
        {
            actions.currentState = CarnivoreActions.CarnivoreStates.SeekFood;
        }

        else if (stats.isEating)
        {
            actions.currentState = CarnivoreActions.CarnivoreStates.EatFood;
        }



        else if (stats.hunger <= 0)
        {
            actions.currentState = CarnivoreActions.CarnivoreStates.Wander;
            stats.isEating = false;
        }

        if (stats.life <= 0)
        {
            Destroy(gameObject);
            animalCount.tigerCount--;
            Debug.Log("Tiger Died");
        }
    }
}
