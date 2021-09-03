using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform foodPrefab;

    [SerializeField]
    private int spawnCount;

    [SerializeField]
    private float spawnVariation;

    private Vector3 foodSourcePosition;


    // Start is called before the first frame update
    void Start()
    {
        foodSourcePosition = this.transform.position;
        SpawnAllFood();
    }



    private void SpawnAllFood()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnFoodFragment();
            
        }
    }

    private void SpawnFoodFragment() 
    {
        float xShift = Random.Range(-spawnVariation, spawnVariation);
        float yShift = Random.Range(-spawnVariation, spawnVariation);
        Vector3 foodPosition = new Vector3(foodSourcePosition.x + xShift, foodSourcePosition.y + yShift, foodSourcePosition.z);
        Transform food = Instantiate(foodPrefab, foodPosition, Quaternion.identity);
        food.SetParent(this.transform);
    }
}
