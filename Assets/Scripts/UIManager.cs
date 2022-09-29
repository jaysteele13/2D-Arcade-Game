using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //variables

    public static UIManager instance;

    public Animator gameOverAnim;

    public Text scoreText, highscoreText, lifeText, trackingScoreText;

    public Image fadeImage;

    private RectTransform scoreRT;

    //private Camera cam;

    int startingScore = 0;
    int highscore = 0;

    //gameover screen
    public Image gameOverImage;
    public Text gameOverText;
    public float gameOverFadeSpeed = 1.5f;
    public GameObject pauseScreen;
   

    //Reference value used for the Smoothdamp method
    //private Vector3 buttonVelocity = Vector3.zero;

    //Smooth time
    //private float smoothTime = 0.5f;





   




    
    private void Awake()
    {
        instance = this;

        scoreRT = trackingScoreText.rectTransform;
        
        //scoreRT = GetComponent<RectTransform>();
    }


    





    private void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0); //declares default save to 0 just in case


        int livesA = GameManager.instance.lives;
        int startingLives = livesA;

        
        



        lifeText.text = startingLives.ToString();
        scoreText.text = startingScore.ToString() + " points";
        highscoreText.text = "highscore: " + this.highscore.ToString();

    }

    public void DisplayPoints(int score)
    {
        scoreText.text = score.ToString() + " points";
        if(this.highscore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
            this.highscore = score;
        }

        //Camera.main.backgroundColor = Color.cyan;
        highscoreText.text = "highscore: " + this.highscore.ToString();
    }

    public void DisplayLives(int lives)
    {
        lifeText.text = lives.ToString();
        
    }


    public void DisplayTrackingScore(Vector3 position, int typeOfScore)
    {
        //display score temporaily at drink location

        trackingScoreText.text = typeOfScore.ToString();

        

        //scoreRT.localPosition = Vector3.SmoothDamp(trackingScoreText.transform.localPosition, position, ref buttonVelocity, smoothTime);

        Debug.Log("Tracking should work");
        //trackingScoreText.gameObject.transform.position = position;

        //INSTANTIATE TEXT AT LOCATION WITH ANIMATION FOR A SECOND WHERE OBJECT WAS DETROYED
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            PlayerPrefs.DeleteAll();

            this.highscore = 0;
            highscoreText.text = "highscore: " + this.highscore.ToString();
            

            
        }

        if (fadeImage.color.a != 0) //black to clear
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.MoveTowards(fadeImage.color.a, 0f, 1f * Time.deltaTime));
        }
    }

    public void DisplayGameOver(bool hasRespawned)
    {
        // gameOverText.color = new Color(gameOverText.color.r, gameOverText.color.g, gameOverText.color.b, Mathf.MoveTowards(gameOverText.color.a, 0f, gameOverFadeSpeed));

        gameOverAnim.SetBool("hasRespawned", hasRespawned);
        
        



    }


    


    /*public IEnumerator LerpScoreObject(Vector3 position, int typeOfScore)
    {
        //experiment of rect transform
        float timeOfTravel = 5; //time after object reach a target place 
        float currentTime = 0; // actual floting time 
        float normalizedValue;


        trackingScoreText.text = typeOfScore.ToString();



        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 

            scoreRT.anchoredPosition = Vector3.Lerp(scoreRT.anchoredPosition, position, normalizedValue);
            yield return null;
        }
    }

    */





}
