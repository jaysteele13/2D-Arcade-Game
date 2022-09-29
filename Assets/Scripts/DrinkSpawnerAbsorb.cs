using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSpawnerAbsorb : MonoBehaviour
{
    //Script Dupe Ignore
    public static DrinkSpawnerAbsorb instance;
    public DrinksAbsorb drinkAbsorbPrefab;

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
        for (int i = 0; i < this.spawnAmount; i++)
        {

            Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance; // edge of circle
            Vector3 spawnPoint = this.transform.position + spawnDirection; //position

            float variance = Random.Range(-this.trajectoryVarience, this.trajectoryVarience);

            Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);

            DrinksAbsorb drinks = Instantiate(this.drinkAbsorbPrefab, spawnPoint, rotation);
            drinks.size = Random.Range(drinks.minSize, drinks.maxSize);

            drinks.SetTrajectory(rotation * -spawnDirection);

            /*var drinkClone = drinks; //make code work

            if(GameManager.instance.shouldDestroyDrinks)
            {
                Destroy(drinkClone);
            }
            */
        }
    }


    public void DestroyStopDrinksAbsorb(bool destroy)
    {

        if(destroy)
        {
            CancelInvoke(nameof(Spawn));
            GameObject[] drinks;

            drinks = GameObject.FindGameObjectsWithTag("Drink Absorb"); // maybe be better alternative
            // code for destroying all drinks
            foreach (GameObject drink in drinks)
            {
                Destroy(drink);
            }
        }
        else
        {
            InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate); //repeat spawning
        }
    }
}
