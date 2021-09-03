using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntHomePathPlacer : MonoBehaviour
{

    #region Seralized Variables
    [SerializeField]
    private float timeBetweenPoints;
    [SerializeField]
    private int pathMaxDisplayLength;
    #endregion

    #region Local Variables
    private LineRenderer lineRenderer;
    private List<Vector3> allPreviousPositions;
    private float timeLeftToPlacePoint;

    #endregion

    #region Initialisers
    private void SetLineRenderer() 
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    private void SetDefaultValues()
    {
        allPreviousPositions = new List<Vector3>();
        timeLeftToPlacePoint = 0;
    }

    #endregion

    #region Updating Time Left
    private void ResetTimeLeft()
    {
        timeLeftToPlacePoint = timeBetweenPoints;
    }

    private void DecrementTimeLeft()
    {
        timeLeftToPlacePoint -= Time.deltaTime;
    }

    #endregion

    #region Home Path Placer


    private void PlacePoint() 
    {
        RecordPosition();
        int[] lineRendererRange = GetLineRendererPathRange();
        UpdateLineRender(lineRendererRange);
    }

    private void RecordPosition() 
    {
        allPreviousPositions.Add(transform.position);
    }

    private int[] GetLineRendererPathRange() 
    {
        int[] lineRendererRange = new int[2];
        int startIndex = 0;
        int length = allPreviousPositions.Count;

        if (length >= pathMaxDisplayLength)
        {
            startIndex = allPreviousPositions.Count - pathMaxDisplayLength;
            length = pathMaxDisplayLength;
        }
        lineRendererRange[0] = startIndex;
        lineRendererRange[1] = length;

        return lineRendererRange;
    }

    private void UpdateLineRender(int[] lineRendererRange) 
    {
        Vector3[] lineRendererPositions = allPreviousPositions.GetRange(lineRendererRange[0], lineRendererRange[1]).ToArray();
        lineRenderer.positionCount = lineRendererPositions.Length;
        lineRenderer.SetPositions(lineRendererPositions);
    }

    private void HandleLinePlacement() 
    {
        if (timeLeftToPlacePoint <= 0) {
            ResetTimeLeft();
            PlacePoint();
        }
    }
    #endregion

    #region Path Public Methods
    public List<Vector3> GetPathHome() 
    {
        return new List<Vector3>(allPreviousPositions);
    }

    public void ResetPathHome()
    {
        allPreviousPositions = new List<Vector3>();
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        SetLineRenderer();
        SetDefaultValues();
    }

    // Update is called once per frame
    void Update()
    {
        HandleLinePlacement();
        DecrementTimeLeft();
    }


}
