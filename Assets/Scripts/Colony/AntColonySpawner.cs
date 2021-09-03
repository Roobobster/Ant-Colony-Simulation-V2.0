using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColonySpawner : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private Transform antPrefab; 

    [SerializeField]
    private int spawnCount;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float spawnDelay;

    [SerializeField]
    private float antMoveSpeed;

    #endregion

    #region Local Variables

    private float timeTillNextSpawn;

    private Vector3 colonyPosition;

    #endregion

    #region Ant Spawning
    private void HandleSpawning(){  
        if(duration > 0){
            timeTillNextSpawn -= Time.deltaTime;
            duration -= Time.deltaTime;
            if(timeTillNextSpawn <= 0 ){
                SpawnAnts();
                timeTillNextSpawn = spawnDelay;
            }
        }
    }

    private void SpawnAnts()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Transform ant = Instantiate(antPrefab, colonyPosition, Quaternion.identity);
            ant.SetParent(this.transform);
            ant.GetComponent<AntMovement>().SetMoveSpeed(antMoveSpeed);
            
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        colonyPosition = GetComponent<Transform>().position;
        timeTillNextSpawn = spawnDelay;
    }

    // Update is called once per frame
    void Update()
    {
        HandleSpawning();

    }

}
