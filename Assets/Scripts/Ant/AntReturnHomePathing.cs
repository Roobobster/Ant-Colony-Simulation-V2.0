using System.Collections.Generic;
using UnityEngine;

public class AntReturnHomePathing : MonoBehaviour
{

    #region Serialized Fields
    [SerializeField]
    private float turnSpeedModifier;
    [SerializeField]
    private float timeAllowedToExplore;
    #endregion

    #region Private Variables
    private int pathHomeIndex;
    private List<Vector3> pathHome;
    private float timeLeftToExplore;
    private bool isHungry;
    #endregion

    #region Updating Time Left To Explore
    private void ResetTimeLeftToExplore()
    {
        isHungry = false;
        timeLeftToExplore = timeAllowedToExplore + Random.Range(1f, 5f);
    }

    private void UpdateTimeLeft() 
    {
        timeLeftToExplore -= Time.deltaTime;
        if (timeLeftToExplore < 0) 
        {
            isHungry = true;
        }
    }

    #endregion

    #region Initialising Variables


    #endregion

    #region Picking Up Food Logic
    private void HandleFoodPicking(Collider2D collision)
    {
        if (CanPickUpThis(collision))
        {
            isHungry = false;
            collision.transform.SetParent(transform);
            collision.gameObject.SetActive(false);
            pathHome = GetComponent<AntHomePathPlacer>().GetPathHome();
            pathHomeIndex = pathHome.Count - 1;
            StopRandomMovement();
            StartPlacingFoodSourcePath();
        }
    }

    private bool CanPickUpThis(Collider2D collision) 
    {
        return collision.CompareTag("Food") && collision.transform.parent.CompareTag("Food") && transform.childCount == 1;
    }

    private void StopRandomMovement()
    {
        GetComponent<AntExplorePathing>().DisableExploring();
        GetComponent<AntMovement>().SetTurnSpeedModifier(turnSpeedModifier);

    }

    private void StartPlacingFoodSourcePath() 
    {
        GetComponent<AntFoodSourcePathPlacer>().EnablePathPlacing();
    }

    #endregion

    #region Returning Home

    public bool isReturningHome() 
    {
        return pathHome != null;
    }

    private void StopReturningHome() 
    {
        ResetTimeLeftToExplore();
        pathHomeIndex = 0;
        pathHome = null;
        GetComponent<AntHomePathPlacer>().ResetPathHome();
        StopPlacingPath();

    }

    private void StopPlacingPath() 
    {
        GetComponent<AntFoodSourcePathPlacer>().DisablePathPlacing();
    }
    


    private void HandleReturningHome()
    {
        if (isReturningHome())
        {
            RotateHome();
            UpdatePathFollow();     
        }
        else if (isHungry) 
        {
            pathHome = GetComponent<AntHomePathPlacer>().GetPathHome();     
        }
        else // If not already going home and explore use food and time allowed to explore
        {
            UpdateTimeLeft(); 
        }
    }

    private void UpdatePathFollow() 
    {
        Vector3 currentFollowPoint = pathHome[pathHomeIndex];

        float distanceToPoint = (transform.position - currentFollowPoint).magnitude;

        if (distanceToPoint < 1f) 
        {
            pathHomeIndex--;
            if (pathHomeIndex < 0) {
                StopReturningHome();
                GetComponent<AntExplorePathing>().EnableExploring();
                if (transform.childCount == 2) 
                {
                    DepositFood();
                }

                GetComponent<AntFollowFoodSourcePathing>().Smell();

            }
        }
    }

    private void DepositFood() 
    {
        Destroy(transform.GetChild(1).gameObject);
    }

    private void RotateHome() 
    {
        Vector3 nextHomePathPoint = pathHome[pathHomeIndex];

        Vector3 moveDirection = nextHomePathPoint  - transform.position;

        GetComponent<AntMovement>().RotateAnt(moveDirection);
    }


    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleFoodPicking(collision);
    }


    // Start is called before the first frame update
    void Start()
    {
        ResetTimeLeftToExplore();

    }

    // Update is called once per frame
    void Update()
    {
        HandleReturningHome();
    }
}
