using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntFoodSourcePathPlacer : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private float timeBetweenPoints;
    [SerializeField]
    private float scentExpireModifier;
    #endregion

    #region Local Variables
    private float scentDuration;
    private float timeLeftToPlacePoint;
    private bool isPlacingPath;
    private Transform world;
    private int currentScentStrength;
    #endregion

    #region Store Path Point
    private void HandleFoodSourcePathPlacing() 
    {
        if (timeLeftToPlacePoint <= 0) 
        {
            ResetTime();
            PlacePoint();
            
            
        }
    }

    private void PlacePoint() 
    {
        currentScentStrength = (int) (currentScentStrength -1); // Reduces strength by 10% each time
        
        world.GetComponent<WorldScentHandler>().AddScent(transform.position, scentDuration, currentScentStrength);
    }
    #endregion 

    #region Update Time Left

    private void DecrementTimeLeft() 
    {
        timeLeftToPlacePoint -= Time.deltaTime;
    }

    private void ResetTime() 
    {
        timeLeftToPlacePoint = timeBetweenPoints;
    }
    #endregion

    #region Initialisation
    private void SetDefaultValues() 
    {
        timeLeftToPlacePoint = 0;
        isPlacingPath = false;
        world = transform.parent.parent.parent; // ant -> colony -> world contents -> world
    }
    #endregion

    #region Toggle Path Placing

    private void EnableLineRenderer() 
    {
        GetComponent<LineRenderer>().enabled = true;
    }

    private void DisableLineRenderer()
    {
        GetComponent<LineRenderer>().enabled = false;
    }
    private void setScentProperties() 
    {
        float distanceFromHome = (transform.parent.position - transform.position).magnitude;
        float timeToGetHomeEstimate = distanceFromHome / (GetComponent<AntMovement>().GetMoveSpeed() / Time.fixedDeltaTime);
        int pointsBetweenHomeEstimate = (int) (timeToGetHomeEstimate / timeBetweenPoints);


        currentScentStrength = (int)( world.GetComponent<WorldScentHandler>().GetMaxDistance() / (distanceFromHome)) + pointsBetweenHomeEstimate;
        scentDuration = timeToGetHomeEstimate * scentExpireModifier;

    }
    public void EnablePathPlacing() 
    {
        setScentProperties();
        isPlacingPath = true;
        EnableLineRenderer();
    }

    public void DisablePathPlacing() 
    {
        isPlacingPath = false;
        DisableLineRenderer();
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
        if (isPlacingPath) 
        {
            DecrementTimeLeft();
            HandleFoodSourcePathPlacing();
        }

    }
}
