using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntFollowFoodSourcePathing : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]
    float smellRadius;
    [SerializeField]
    float smellInterval;
    #endregion

    #region Local Variables
    private float timeLeftToSmell;
    private Transform world;
    #endregion

    #region Updating Time Left
    private void ResetTime() 
    {
        timeLeftToSmell = smellInterval + Random.Range(0f, 0.1f);
    }

    private void DecrementTimeLeft() 
    {
        timeLeftToSmell -= Time.deltaTime;
    }
    #endregion

    #region Check For Smell
    private void CheckForSmell() 
    {
        if (timeLeftToSmell <= 0) 
        {
            Smell();

        }
    }

    public void Smell() 
    {
        Scent strongestScent = world.GetComponent<WorldScentHandler>().GetScentNearby(transform.position, smellRadius);
        if (!GetComponent<AntReturnHomePathing>().isReturningHome() && strongestScent != null && strongestScent.GetStrength() > 0)  // Checks for null incase ant gets out of grid somehow
        {
            Vector3 moveDirection = strongestScent.GetLocation() - transform.position;
            GetComponent<AntMovement>().RotateAnt(moveDirection);
            GetComponent<AntMovement>().SetTurnSpeedModifier(10f);

        }
        ResetTime();
    }
    #endregion

    #region Initialisation
    private void SetDefaultValues() 
    {
        world = transform.parent.parent.parent; // ant -> colony -> world contents -> world
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementTimeLeft();
        CheckForSmell();
    }
}
