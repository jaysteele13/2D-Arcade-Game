using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    private Vector3 drinkPosition = Vector3.one;

    public int lives = 3;
    private int startingLives = 0; 
    public float respawnTime = 3.0f, invisibilityTime = 2.0f;
    public ParticleSystem explosion;

    public float minDrinkSizeMultiplier = 0.9f, midDrinkSizeMultiplier = 1.25f;

    public int score = 0;

    public int smallScore = 25, midScore = 50, bigScore = 100, hugeScore = 200; //type of score

    [HideInInspector]
    public bool shouldDestroyDrinks = false;
    public int typeOfScore = 0;
    public bool playerDead;
    //public bool playerAlive;

    //stuff to do to finish the game
    //add anomther pug enemy (friday)
    //make walls move slowly and randomly after 2000 points ( thurday)
    //add more intresting erins (thursday or friday)
    //roam and chase player when pug gets close
    // make simple music for game
    
    
    //Prefab for scores
    public GameObject smallScoreIMG, midScoreIMG, bigScoreIMG, hugeScoreIMG;

    private void Awake()
    {
        instance = this;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;

        this.startingLives = this.lives;
    }
    

    public void PugDestroyed(PugChasing pug)
    { 

        this.explosion.transform.position = pug.transform.position;
        this.explosion.Play();

        

        //do score here

        if (pug.randomScale >= 0.3f)
        {
            this.score += midScore;
            this.typeOfScore = this.midScore;
        }
        else if (pug.randomScale > 0.2f)
        {
            this.score += bigScore;
            this.typeOfScore = this.bigScore;
        }
        



        UIManager.instance.DisplayPoints(score); //only add score for stuff that can be absorbed!
                                            //UIManager.instance.DisplayTrackingScore(this.drinkPosition, this.typeOfScore);
        WallManager.instance.WallsMoveOnScore(this.score); // make walls move depending on current score!
        ShowScoreLive(this.typeOfScore, pug);

    }

    //dupe drinks Destroyed
    public void DrinkDestroyedAbsorb(DrinksAbsorb drink)
    {
        
        this.explosion.transform.position = drink.transform.position;
        this.explosion.Play(); 

        this.drinkPosition = drink.transform.position;

        //do score here

        if (drink.size < minDrinkSizeMultiplier)
        {
            this.score += bigScore;
            this.typeOfScore = this.bigScore;

            
        }
        else if (drink.size < midDrinkSizeMultiplier)
        {
            this.score += midScore;
            this.typeOfScore = this.midScore;
        }
        else
        {
            this.score += smallScore;
            this.typeOfScore = this.smallScore;
        }

        //get physics for when shot there is no multiplier, when absorbed, there is - create bool to be able to tell
        WallManager.instance.WallsMoveOnScore(this.score); // make walls move depending on current score!

        UIManager.instance.DisplayPoints(score); //only add score for stuff that can be absorbed!
        //UIManager.instance.DisplayTrackingScore(this.drinkPosition, this.typeOfScore);
        ShowScoreLive(this.typeOfScore, drink);


        //StartCoroutine(UIManager.instance.LerpScoreObject(this.drinkPosition, this.typeOfScore)); //test to track score, lerp works

    }
    //ignore this dupe

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            Debug.Log("Paused");
            PauseUnPause()
;            //Time.timeScale = 0;


        }
    }

    public void PauseUnPause()
    {
        if(UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked; //freedom of mouse
            Cursor.visible = false;

            Time.timeScale = 1f;

        }
        else if (!UIManager.instance.pauseScreen.activeInHierarchy)
        {



            UIManager.instance.pauseScreen.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;



        }
    }

    public void PlayerDied()
    {
        
        this.lives--;

        //Player.instance.TurnOnOffBulletInvoke(false); // turn off bullet


        playerDead = true; // tells if player dead

        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        UIManager.instance.DisplayLives(this.lives);

        

        if (this.lives <= 0)
        {
            GameOver();
        }
        else
        {
            //DrinkSpawner.instance.DestroyStopDrinks(true);
           // DrinkSpawnerAbsorb.instance.DestroyStopDrinksAbsorb(true); //stopping drinks and destroying

            //shouldDestroyDrinks = true;

            Invoke(nameof(Respawn), this.respawnTime);
            DrinkSpawner.instance.DestroyStopDrinks(true);
            DrinkSpawnerAbsorb.instance.DestroyStopDrinksAbsorb(true); //stopping drinks and destroying
        }

    }

    

    public void Respawn()
    {
        shouldDestroyDrinks = false; // to control destroying drinks in drinkspawners

        //Player.instance.TurnOnOffBulletInvoke(true); // after death resumes random shooting

        UIManager.instance.DisplayGameOver(true); //game over scene
        //InvokeRepeating(nameof(WallManager.instance.turnWall1), 0f, WallManager.instance.delayWallTime);

        playerDead = false;

        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions"); //turn off player collisions

        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);

        Invoke(nameof(TurnOnCollisions), this.invisibilityTime);

        DrinkSpawner.instance.DestroyStopDrinks(false);
        DrinkSpawnerAbsorb.instance.DestroyStopDrinksAbsorb(false); //so code doesb't constantly repeat - test to make sure it works!

        
        if (this.lives == startingLives) //to delay the lives being 0 so suer knows
        {
            UIManager.instance.DisplayLives(this.lives);
            DrinkSpawner.instance.DestroyStopDrinks(false);
            DrinkSpawnerAbsorb.instance.DestroyStopDrinksAbsorb(false); //so code doesb't constantly repeat - test to make sure it works!
            
        }

        if(this.score == 0) //to delay points from dissapearing so player gets the chance to see what they got
        {
            UIManager.instance.DisplayPoints(score);
        }
        //WallManager.instance.ResetWalls();
        //CancelInvoke(nameof(WallManager.instance.moveRightWall));
    }

    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void GameOver()
    {
        DrinkSpawner.instance.DestroyStopDrinks(true);
        DrinkSpawnerAbsorb.instance.DestroyStopDrinksAbsorb(true); //stopping drinks and destroying

        UIManager.instance.DisplayGameOver(false); // game over scene // maybe make a method in UI controller for this!

        Invoke(nameof(Respawn), this.respawnTime);
        this.lives = startingLives;
        this.score = 0;

        shouldDestroyDrinks = true;

        CancelInvoke(nameof(Player.instance.Shoot));
        //Invoke(nameof(ResetDrinks), this.respawnTime);

        playerDead = true;

        //CancelInvoke(nameof(WallManager.instance.moveRightWall)); // move RightWall from wall manager 
        WallManager.instance.ResetWalls();


    }

    public void ShowScoreLive(int typeOfScore, DrinksAbsorb drink)
    {
        this.drinkPosition = drink.transform.position;
        float destroyDrink = 1.5f;

        if (typeOfScore == smallScore)
        {
            //instantiate anims at position
            //destroy after couple of seconds / thats what anim does
            var smallClone = Instantiate(smallScoreIMG, drink.transform.position, Quaternion.identity);
            Destroy(smallClone.gameObject, destroyDrink);
        }
        else if(typeOfScore == midScore)
        {
            var midClone = Instantiate(midScoreIMG, drink.transform.position, Quaternion.identity); 
            Destroy(midClone, destroyDrink);
        }
        else if(typeOfScore == bigScore)
        {
            var bigClone = Instantiate(bigScoreIMG, drink.transform.position, Quaternion.identity); //need the var to dodge error!!
            Destroy(bigClone, destroyDrink);
        }
        else
        {
            var hugeClone = Instantiate(hugeScoreIMG, drink.transform.position, Quaternion.identity); //need the var to dodge error!!
            Destroy(hugeClone, destroyDrink);
        }
        
    }

    public void ShowScoreLive(int typeOfScore, PugChasing pug)
    {
        this.drinkPosition = pug.transform.position;
        float destroyDrink = 1.5f;

        if (typeOfScore == smallScore)
        {
            //instantiate anims at position
            //destroy after couple of seconds / thats what anim does
            var smallClone = Instantiate(smallScoreIMG, pug.transform.position, Quaternion.identity);
            Destroy(smallClone.gameObject, destroyDrink);
        }
        else if (typeOfScore == midScore)
        {
            var midClone = Instantiate(midScoreIMG, pug.transform.position, Quaternion.identity);
            Destroy(midClone, destroyDrink);
        }
        else if (typeOfScore == bigScore)
        {
            var bigClone = Instantiate(bigScoreIMG, pug.transform.position, Quaternion.identity); //need the var to dodge error!!
            Destroy(bigClone, destroyDrink);
        }
        else
        {
            var hugeClone = Instantiate(hugeScoreIMG, pug.transform.position, Quaternion.identity); //need the var to dodge error!!
            Destroy(hugeClone, destroyDrink);
        }

    }


    public void ResetPug()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }



    
}
