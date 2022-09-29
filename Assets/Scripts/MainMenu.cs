using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image fadeImage;
    public float playDelay;

    private bool fade = false;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(LoadLevelAfterDelay(playDelay));
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /*IEnumerator LoadBlackScreen()
    {
        for(float i = 0.07f; i <= 1.1; i+= 0.07f)
        {
            yield return new WaitForSeconds(0.1f);
            fadeImage.color = new Color(0f, 0f, 0f, i);
        }
        
    }
    */


    // Update is called once per frame
    void Update()
    {
        if (fadeImage.color.a != 1 && fade)
        {
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, Mathf.MoveTowards(fadeImage.color.a, 1f, playDelay * Time.deltaTime));
        }
        
    }

    public void QuitGame()
    {
        Application.Quit(); //might work - dk
    }

    public void PlayGame()
    {
        //StartCoroutine(LoadBlackScreen());
        fade = true;
        StartCoroutine(LoadLevelAfterDelay(playDelay));
    }

    /*public Color LerpBlack()
    {
        return Color.Lerp(Color.white, Color.black, Mathf.Sin(Time.time));
    }
    */
    

}

