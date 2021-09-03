using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScentHandler : MonoBehaviour
{
    #region Local Variables
    //x,y
    private Scent[][] antFoodSourcePathPoints;
    private HashSet<Scent> pathPointsNeedUpdating;


    private float width;
    private float height;
    private float startXPosition;
    private float startYPosition;
    #endregion

    #region Serialized Variables
    [SerializeField]
    private float cellSize;
    [SerializeField]
    private Transform gridCell;

    #endregion

    #region Public Methods
    public void AddScent(Vector3 location, float scentDuration, int scentStrength)
    {
        int[] gridLocation = GetGridLocation(location);
        Scent scent = antFoodSourcePathPoints[gridLocation[0]][gridLocation[1]];
        scent.AddScent(scentDuration, scentStrength);
        pathPointsNeedUpdating.Add(scent);

    }
    public Scent GetScentNearby(Vector3 location, float scentRadius)
    {
        int[] currentGridLocation = GetGridLocation(location);

        int[] xBounds = GetXBounds(currentGridLocation[0], scentRadius);
        int[] yBounds = GetYBounds(currentGridLocation[1], scentRadius);

        Scent strongestScent = GetStrongestScent(xBounds, yBounds);


        return strongestScent;

    }

    private Scent GetStrongestScent(int[] xBounds, int[] yBounds) 
    {
        Scent strongestScent = null;

        for (int x = xBounds[0]; x <= xBounds[1]; x++)
        {
            for (int y = yBounds[0]; y <= yBounds[1]; y++)
            {
                Scent currentScent = antFoodSourcePathPoints[x][y];
                if (strongestScent == null || strongestScent.GetStrength() < currentScent.GetStrength())
                {
                    strongestScent = currentScent;
                }
            }
        }
        

        return strongestScent;
    }

    public float GetMaxDistance() 
    {
        return width > height ? width : height;
    }
    #endregion

    #region Scent Handling
    private void UpdateWorldScents() 
    {
        List<Scent> scentsToRemove = new List<Scent>();
        foreach (Scent scent in pathPointsNeedUpdating) 
        {
            bool hasExpired = scent.ReduceScentDuration(Time.deltaTime);
            if (hasExpired)
            {
                scentsToRemove.Add(scent);
            } 
        }

        foreach (Scent scent in scentsToRemove)
        {
            pathPointsNeedUpdating.Remove(scent);
        }


    }

    #endregion


    #region Getting Grid Location 

    private int[] GetGridLocation(Vector3 location)
    {

        int scentGridXLocation = (int)((location.x - startXPosition) / cellSize);
        int scentGridYLocation = (int)((location.y - startYPosition) / cellSize);
        int[] gridLocation = new int[] { scentGridXLocation, scentGridYLocation };

        return gridLocation;
    }


    private int[] GetXBounds(int xCentre, float scentRadius) 
    {
        int lowerBoundX = (int)(xCentre - scentRadius) < 0 ? 0 : (int)(xCentre - scentRadius);
        int upperBoundX = (int)(xCentre + scentRadius) >= antFoodSourcePathPoints.Length ? antFoodSourcePathPoints.Length - 1 : (int)(xCentre + scentRadius);
        int[] bounds = new int[] { lowerBoundX, upperBoundX };
        return bounds;
    }

    private int[] GetYBounds(int yCentre, float scentRadius)
    {
        int lowerBoundY = (int)(yCentre - scentRadius) < 0 ? 0 : (int)(yCentre - scentRadius);
        int upperBoundY = (int)(yCentre + scentRadius) >= antFoodSourcePathPoints[0].Length ? antFoodSourcePathPoints[0].Length - 1 : (int)(yCentre + scentRadius);
        int[] bounds = new int[] { lowerBoundY, upperBoundY };
        return bounds;
    }
    #endregion


    #region Initialisation
    private void SetDefaultValues() 
    {
        SetGridVariables();
        CreateScentGrid();
    }

    private void SetGridVariables() 
    {
        pathPointsNeedUpdating = new HashSet<Scent>();

        Transform gridBounds = transform.GetChild(0);
        width = gridBounds.localScale.x;
        height = gridBounds.localScale.y;
        startXPosition = gridBounds.position.x - (gridBounds.localScale.x/2) ;
        startYPosition = gridBounds.position.y - (gridBounds.localScale.y / 2);
    }

    private void CreateScentGrid() 
    {
        int totalRows = (int) (height / cellSize) + 1; // + 1 ensures whole width and height is covered.
        int totalColumns = (int)(width / cellSize) + 1;
        antFoodSourcePathPoints = new Scent[totalColumns][];
        for (int x = 0; x < totalColumns; x++)
        {
            antFoodSourcePathPoints[x] = new Scent[totalRows];
            for (int y = 0; y < totalRows; y++)
            {
                float xPosition = (x * cellSize) + startXPosition;
                float yPosition = (y * cellSize) + startYPosition;
                Vector3 cellLocation = new Vector3(xPosition, yPosition, 0);
                
                antFoodSourcePathPoints[x][y] = new Scent(cellLocation, 0);
            }
        }
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

        UpdateWorldScents();
    }
}

public class Scent 
{
    private Vector3 location;

    private float scentDuration;

    private int strength;

    public Scent(Vector3 location , float scentDuration)
    {
        this.scentDuration = scentDuration;
        this.location = location;
        strength = 0;
    }

    public void AddScent(float scentDuration, int scentStrength) {
        this.scentDuration = this.scentDuration < scentDuration ? scentDuration : this.scentDuration; // Sets longer duration
        this.strength = this.strength < scentStrength ? scentStrength : this.strength; // Sets larger strength
    }

    public int GetStrength() { return strength; }

    public Vector3 GetLocation() { return location; }

    public bool ReduceScentDuration(float timeReduction)
    { 
        scentDuration = scentDuration - timeReduction < 0 ? 0 : scentDuration - timeReduction; // Reduces scent down to 0
        strength = scentDuration == 0 ? 0 : strength;
        return scentDuration == 0 ? true : false;

    } 
}