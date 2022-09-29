using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSpawner : MonoBehaviour
{
    public static DrinkSpawner instance;

    public PugChasing pugPrefab;
    

    public float spawnRate = 2.0f;
    public float trajectoryVarience = 15.0f;

    public int spawnAmount = 1;
    public float spawnDistance = 15.0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate); //repeat spawning
    }


    private void Spawn()
    {
        for(int i = 0; i < this.spawnAmount; i++)
        {
            
            if (!GameManager.instance.playerDead)
            {
                Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance; // edge of circle
                Vector3 spawnPoint = this.transform.position + spawnDirection; //position

                float variance = Random.Range(-this.trajectoryVarience, this.trajectoryVarience);

                Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

                PugChasing pugChasing = Instantiate(this.pugPrefab, spawnPoint, rotation);

                
            }
        }
    }

    public void DestroyStopDrinks(bool destroy)
    {

        if (destroy)
        {
            CancelInvoke(nameof(Spawn));
            // code for destroying all drinks
            GameObject[] pugChasing;

            pugChasing = GameObject.FindGameObjectsWithTag("Pug"); // maybe be better alternative
            // code for destroying all drinks
            foreach (GameObject PugChasing in pugChasing)
            {
                Destroy(PugChasing);
            }
        }
        else
        {
            InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate); //repeat spawning
        }
    }


}
