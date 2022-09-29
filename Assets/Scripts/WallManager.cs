using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static WallManager instance;
    
    
    public float turnSpeed;
    public GameObject[] walls = new GameObject[4]; //there will only be 4 walls!
    public GameObject[] innerWalls = new GameObject[4]; //there will only be 4 walls!
    private int[] usedNos = new int[4];

    private int tempScore; //used for fixedupdate for score


    [HideInInspector]
    //public int currentScore;
    //wall vars
    private float horizontalTurn = 0f, startingHorizontalTurn = 0f, startingVerticalTurn = 0f, verticalTurn = 90f, horizontalOffset = 1.5f, verticalOffset = 5f;
    private bool allowLeft, allowRight, allowMiddle = true, allowMiddleAgain;



    private int randomNo = 0;
    public bool rotateLeft, rotateReset, rotateRight;

    public float delayWallTime = 1f;

    private Vector3 startingLeftWall, startingTopWall, startingBottomWall, startingRightWall, startingLeftScale, startingTopScale, startingBottomScale, startingRightScale;

    private bool wallIsRight2ndStage = true;

    private bool whenToInvoke = false, repeat700 = true;

    //public int wallCounter = 0; // check if wall sequence has been called
 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        startingHorizontalTurn = horizontalTurn;
        startingVerticalTurn = verticalTurn;

        InvokeRepeating(nameof(turnWall1), 0f, delayWallTime); // code works just need to repeat smoothly and adapt speed and movement when more points are scored!
        //InvokeRepeating(nameof(moveRightWall), 0f, 10f);

        startingLeftWall = new Vector3(walls[0].transform.position.x, walls[0].transform.position.y, 0f);
        startingTopWall = new Vector3(walls[1].transform.position.x, walls[1].transform.position.y, 0f);
        startingBottomWall = new Vector3(walls[2].transform.position.x, walls[2].transform.position.y, 0f);
        startingRightWall = new Vector3(walls[3].transform.position.x, walls[3].transform.position.y, 0f);

        startingLeftScale = new Vector3(walls[0].transform.localScale.x, walls[0].transform.localScale.y, 0f);
        startingTopScale = new Vector3(walls[1].transform.localScale.x, walls[1].transform.localScale.y, 0f);
        startingBottomScale = new Vector3(walls[2].transform.localScale.x, walls[2].transform.localScale.y, 0f);
        startingRightScale = new Vector3(walls[3].transform.localScale.x, walls[3].transform.localScale.y, 0f);
    }



    

    public void WallsMoveOnScore(int currentScore)
    {



        //
        
        if (currentScore >250 || (currentScore % 250 == 0))
        {
            //move all walls in a bit
            /*walls[0].transform.position = newLeftPos; // use lean scale to make it look elegant
            walls[1].transform.position = newTopPos;
            walls[2].transform.position = newBottomPos;
            walls[3].transform.position = newRightPos;
            
            */
            StartCoroutine(closeAndOpenWalls(currentScore/250)); 

            //InvokeRepeating("WallsMove", startingWaitForWall, startingWaitForWall * 2);

            //StartCoroutine(closeAndOpenWalls(1f));


            //InvokeRepeating("ResetWalls", startingWaitForWall, startingWaitForWall * 2);
            //changing position


            //invoke repeating (thus code, 1000 / 250

            //randomNo = Random.Range(0, 4);
            /*
            walls[0].transform.LeanMove(innerWalls[0].transform.position, 3f); // l
            walls[1].transform.LeanMove(innerWalls[1].transform.position, 3f); //t
            walls[2].transform.LeanMove(innerWalls[2].transform.position, 3f); //b
            walls[3].transform.LeanMove(innerWalls[3].transform.position, 3f); //r


            walls[0].transform.LeanScale(innerWalls[0].transform.localScale, 3f); // l
            walls[1].transform.LeanScale(innerWalls[1].transform.localScale, 3f); // t
            walls[2].transform.LeanScale(innerWalls[2].transform.localScale, 3f); // b
            walls[3].transform.LeanScale(innerWalls[3].transform.localScale, 3f); // r

            repeat700 = false; //stopping 700 score code
            */

        }

    }



   


    public void WallsMove() {

        walls[0].transform.LeanMove(innerWalls[0].transform.position, 3f); // l
        walls[1].transform.LeanMove(innerWalls[1].transform.position, 3f); //t
        walls[2].transform.LeanMove(innerWalls[2].transform.position, 3f); //b
        walls[3].transform.LeanMove(innerWalls[3].transform.position, 3f); //r


        walls[0].transform.LeanScale(innerWalls[0].transform.localScale, 3f); // l
        walls[1].transform.LeanScale(innerWalls[1].transform.localScale, 3f); // t
        walls[2].transform.LeanScale(innerWalls[2].transform.localScale, 3f); // b
        walls[3].transform.LeanScale(innerWalls[3].transform.localScale, 3f); // r

    }

    public IEnumerator closeAndOpenWalls(float delayTime)
    {
        WallsMove();
        yield return new WaitForSeconds(delayTime);
        ResetWalls();
    }


   /* public void moveRightWall()
    {
       
        if(wallIsRight2ndStage)
        {
            walls[3].transform.LeanMove(innerWalls[7].transform.position, 10f); //r
            wallIsRight2ndStage = false;
        }
        else
        {
            walls[3].transform.LeanMove(innerWalls[3].transform.position, 10f); //r
            wallIsRight2ndStage = true;
        }
        
        
    }
   */


    public void Tween()
    {
        randomNo = Random.Range(0, (walls.Length - 1));
        LeanTween.cancel(walls[randomNo].gameObject);

        transform.localScale = Vector3.one;

        LeanTween.scale(walls[randomNo], Vector3.one * 2f, turnSpeed)
            .setEasePunch();

        

    }

    

    public void turnWall1()
    {
        
        if(allowLeft)
        {
            walls[0].transform.LeanRotateZ((verticalTurn - verticalOffset), Time.deltaTime * turnSpeed); //0 and 3 index are vertical (85 - 95) the other are horizontal(-1.5)
            walls[1].transform.LeanRotateZ((horizontalTurn - horizontalOffset), Time.deltaTime * turnSpeed);
            walls[2].transform.LeanRotateZ((horizontalTurn - horizontalOffset), Time.deltaTime * turnSpeed);
            walls[3].transform.LeanRotateZ((verticalTurn - verticalOffset), Time.deltaTime * turnSpeed);

            allowLeft = false;
            allowMiddleAgain = true;
        }
        else if(allowRight)
        {
            walls[0].transform.LeanRotateZ((verticalTurn + verticalOffset), Time.deltaTime * turnSpeed); //0 and 3 index are vertical (85 - 95) the other are horizontal(-1.5)
            walls[1].transform.LeanRotateZ((horizontalTurn + horizontalOffset), Time.deltaTime * turnSpeed);
            walls[2].transform.LeanRotateZ((horizontalTurn + horizontalOffset), Time.deltaTime * turnSpeed);
            walls[3].transform.LeanRotateZ((verticalTurn + verticalOffset), Time.deltaTime * turnSpeed);

            allowRight = false;
            allowMiddle = true;
        }
        else if (allowMiddleAgain)
        {
            walls[0].transform.LeanRotateZ((verticalTurn), Time.deltaTime * turnSpeed); //0 and 3 index are vertical (85 - 95) the other are horizontal(-1.5)
            walls[1].transform.LeanRotateZ((horizontalTurn), Time.deltaTime * turnSpeed);
            walls[2].transform.LeanRotateZ((horizontalTurn), Time.deltaTime * turnSpeed);
            walls[3].transform.LeanRotateZ((verticalTurn), Time.deltaTime * turnSpeed);

            allowRight = true;
            allowMiddleAgain = false;

            //Tween(); //lol
        }
        else if (allowMiddle)
        {
            walls[0].transform.LeanRotateZ((verticalTurn), Time.deltaTime * turnSpeed); //0 and 3 index are vertical (85 - 95) the other are horizontal(-1.5)
            walls[1].transform.LeanRotateZ((horizontalTurn), Time.deltaTime * turnSpeed);
            walls[2].transform.LeanRotateZ((horizontalTurn), Time.deltaTime * turnSpeed);
            walls[3].transform.LeanRotateZ((verticalTurn), Time.deltaTime * turnSpeed);


            allowMiddle = false;
            allowLeft = true;

            //Tween(); //for lols


        }

        
    }

    


    public void ResetWalls()
    {
        //CancelInvoke(nameof(moveRightWall));

        walls[0].transform.LeanMove(startingLeftWall, 3f); // l
        walls[1].transform.LeanMove(startingTopWall, 3f); //t
        walls[2].transform.LeanMove(startingBottomWall, 3f); //b
        walls[3].transform.LeanMove(startingRightWall, 3f); //r


        walls[0].transform.LeanScale(startingLeftScale, 3f); // l
        walls[1].transform.LeanScale(startingTopScale, 3f); // t
        walls[2].transform.LeanScale(startingBottomScale, 3f); // b
        walls[3].transform.LeanScale(startingRightScale, 3f); // r
    }

    public static IEnumerator After(IEnumerator coroutine, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        yield return coroutine;
    }
}
