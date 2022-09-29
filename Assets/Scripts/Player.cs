using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public static Player instance;

    public float thrustSpeed = 1.0f, turnSpeed = 1.0f;
    public Bullet bulletPrefab;
    public GameObject firepoint;
    public float waitBetweenShots = 3f, minShootSpeed = 0f, maxShootSpeed = 0;
    private float startingWaitBetweenShots = 0;

    private bool thrusting;
    private float turnDirection;
    private Rigidbody2D rb;
    private Animator anim;
    public float animShootDelay = 0.5f;
    public bool mainMenuMode = false;
    //private bool shooting = false;
    //private float animDelay = 3f;

    [HideInInspector]
    public float randomShootSpeed = 3f;


    private void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startingWaitBetweenShots = waitBetweenShots;
    }

    private void Start()
    {

        this.randomShootSpeed = Random.Range(minShootSpeed, maxShootSpeed);
       
        //InvokeRepeating(nameof(Shoot), this.randomShootSpeed, this.randomShootSpeed); //new animation and game mechanics!! plsss

        Debug.Log(randomShootSpeed);


    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        //thrusting = Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0); // experimental - firerate set for bullets hoot out back but must move in process - try later

        //tomorrow - new art style (undertale!
        //animShootDelay -= Time.deltaTime; // fixing anim

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            turnDirection = 1.0f;
            
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            turnDirection = -1.0f;
        }
        else {
            turnDirection = 0.0f;
        }



        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
            anim.SetBool("ShootingTrue", true);

            //copy website and try it!
        }




    }


    private void FixedUpdate() // good for physics 
    {
        if(thrusting)
        {
            rb.AddForce(this.transform.up * this.thrustSpeed); // 2d forward is this.transform.up
        }

        if(turnDirection != 0)
        {
            rb.AddTorque(turnDirection * this.turnSpeed); //method to turn
        }

        if(mainMenuMode)
        {
              
        }

        /*float currentSpeed = rb.mass * rb.angularVelocity;
        anim.SetFloat("speed", currentSpeed); */
    }

    public void Shoot()
    { 
        Bullet bullet = Instantiate(this.bulletPrefab, this.firepoint.transform.position, this.transform.rotation);
        bullet.Project(-this.transform.up); // bullet go other way?

        //anim.SetBool("shoot", true);
        //this.randomShootSpeed = Random.Range(f, 3f); //keep making bullet appearance random
        //Debug.Log("Here is Random Number: " + randomShootSpeed);

        //set shooting anim true

        Invoke("ResetShoot", animShootDelay); // website anim shoot
        

        

        //wait 2 seconds
        //set to false

    }

    public void ResetShoot()
    {
        anim.SetBool("ShootingTrue", false); // website animshoots
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Drink")
        {
            if (Drinks.instance.canAbsorb == false)
            {

                PlayerDyingPhysics();

            }
        }

        if (collision.gameObject.tag == "Pug")
            {

                PlayerDyingPhysics();

            }

        if(collision.gameObject.tag == "Wall")
        {
            PlayerDyingPhysics();
        }

        if (collision.gameObject.tag == "Drink Absorb")
        {
            Invoke("ResetShoot", animShootDelay); // website anim shoot
            anim.SetBool("ShootingTrue", true);
            
        }
    }

   /* public void TurnOnOffBulletInvoke(bool on)
    {
        if (on) //player dead?
        {
            
            InvokeRepeating(nameof(Shoot), this.randomShootSpeed, this.randomShootSpeed);
        }
    
        else
        {
            CancelInvoke(nameof(Shoot));
        }
    }
   */

    public void PlayerDyingPhysics()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0.0f;

        this.gameObject.SetActive(false);

        GameManager.instance.PlayerDied();
    }



}
