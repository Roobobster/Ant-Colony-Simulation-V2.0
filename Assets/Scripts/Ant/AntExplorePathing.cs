using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntExplorePathing : MonoBehaviour
{

    #region Serialized Variables
    [SerializeField]
    private float turnDegreesMax;
    [SerializeField]
    private float turnTimeMax;
    [SerializeField]
    private float turnTimeMin;
    #endregion

    #region Private Variables
    private float timeLeft;
    private bool isExploring;
    private float rotationRemaining;
    private float rotationDirection;
    #endregion

    #region Exploring Logic

    private void SetRotationDirection()
    {
        rotationDirection = Random.Range(-1, 1);

        if (rotationDirection == 0)
        {
            rotationDirection = 1;
        }
    }

    private void SetRotationRemaining()
    {
        rotationRemaining = Random.Range(0, turnDegreesMax);
    }

    private void HandleRandomTurning()
    {
        if (timeLeft <= 0)
        {
            SetRotationRemaining();
            SetRotationDirection();
            ApplyExploration();
            ResetTimeLeft();
        }
    }

    private void Explore()
    {
        DecrementTimeLeft();
        HandleRandomTurning();
    }
    private void ApplyExploration()
    {
        GetComponent<AntMovement>().SetDirection(rotationRemaining, rotationDirection);
    }


    #endregion

    #region Updating Time To Change Movement
    private void ResetTimeLeft()
    {
        timeLeft = Random.Range(turnTimeMin, turnTimeMax);
    }
    private void DecrementTimeLeft()
    {
        timeLeft -= Time.deltaTime;
    }
    #endregion

    #region Enable/Disable Exploring Movement
    public void EnableExploring()
    {
        isExploring = true;
        GetComponent<AntMovement>().ResetTurnSpeedModifier();
    }

    public void DisableExploring()
    {
        isExploring = false;
    }

    public bool GetIsExploring() { return isExploring; }
    #endregion

    #region Initialisers
    private void SetDefaultValues()
    {
        isExploring = true;
        timeLeft = 0;
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
        if (isExploring)
        {
            Explore();
            
        }

    }
}
