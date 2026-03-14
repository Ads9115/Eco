using UnityEngine;

public class HerbivoreAi : MonoBehaviour
{
    private Agent agent;
    private AnimalsStats stats;
    private HerbivoreActions actions;
   

    private void Start()
    {
        actions = GetComponent<HerbivoreActions>();
        agent = GetComponent<Agent>();
        stats = GetComponent<AnimalsStats>();
    }

    private void Update()
    {

        if (stats.hunger > 60 && !stats.isEating)
        {
            //actions.currentState=HerbivoreActions.HerbivoreStates.SeekFood;
        }

        

        else if (stats.hunger == 0)
        {
            actions.currentState = HerbivoreActions.HerbivoreStates.Wander;
            stats.isEating = false;
        }
    }
}
