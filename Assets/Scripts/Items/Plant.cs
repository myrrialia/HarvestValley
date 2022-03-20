using System.Collections;
using UnityEngine;

public class Plant : Item
{
    [SerializeField] [Tooltip("Delay between growth stages")] private float growthDelay = 3f;
    [SerializeField] [Tooltip("Array of the plant's different growth stages")] private GameObject[] growthStages; 
    private bool harvestable = false;  //Is the plant ready to be harvested
    private int stageIndex = 0; //Used to cycle through the growthStages arrray

    //Harvestable getter
    public bool isHarvestable()
    {
        return harvestable;
    }

    //Start growing
    private void Start()
    {
        CheckGrowth();
    }

    //If player interacts with plant ready to harvest
    public override void Interaction()
    {
        if(harvestable)
            base.Interaction();
    }

    //Checks if the plant is fully grown yet
    private void CheckGrowth()
    {
        if (stageIndex < growthStages.Length - 1)
            StartCoroutine(Grow());
        else
            harvestable = true; //If fully grown, set bool
    }

    //Plant grows over time
    IEnumerator Grow()
    {
        float timer = growthDelay;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        growthStages[stageIndex].SetActive(false);  //Hide old growth stage
        stageIndex++;   //Increase growth stage
        growthStages[stageIndex].SetActive(true);   //Show new growth stage
        CheckGrowth();
    }
}
