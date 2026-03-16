using UnityEngine;

public class HerbivoreAi : MonoBehaviour
{
    private Agent agent;
    private HerbivoreStats stats;
    private HerbivoreActions actions;
   

    private void Start()
    {
        actions = GetComponent<HerbivoreActions>();
        agent = GetComponent<Agent>();
        stats = GetComponent<HerbivoreStats>();
    }

    private void Update()
    {

        if (stats.life <= 0)
            Destroy(gameObject);
        // No tiger check needed here anymore — collider handles it

        if (actions.currentState == HerbivoreActions.HerbivoreStates.Escape)
            return; // don't override escape state

        if (stats.hunger > 60 && !stats.isEating &&
            actions.currentState != HerbivoreActions.HerbivoreStates.SeekFood)
        {
            actions.currentState = HerbivoreActions.HerbivoreStates.SeekFood;
        }
        else if (stats.isEating)
        {
            actions.currentState = HerbivoreActions.HerbivoreStates.EatFood;
        }
        else if (stats.hunger <= 0)
        {
            actions.currentState = HerbivoreActions.HerbivoreStates.Wander;
            stats.isEating = false;
        }

    }
}
