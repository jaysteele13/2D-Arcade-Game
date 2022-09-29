using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PugChasing : MonoBehaviour
{
    public float speed = 5f, distanceToCatch = 6f;
    private Transform player;
    public int pugHealth = 5;
    public float maxLifeTime = 30f;
    private int startingPugHealth;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float distanceToPlayer;
    private Vector3 direction;
    public float randomScale;

    public static PugChasing instance;

   // public static PugChasing instance;

    //private void Awake()
    //{
       // instance = this;
    //}




    public float size = 1.0f, minSize = 0.5f, maxSize = 1.5f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        randomScale = Random.Range(0.2f, 0.5f);
        transform.localScale = new Vector3(randomScale, randomScale, 0f);

        if (randomScale >= 0.4f)
        {
            startingPugHealth = 3;
        }
        else if (randomScale >= 0.3f)
        {
            startingPugHealth = 2;
        }
        else if(randomScale > 0.2f)
        {
            startingPugHealth = 1;
        }

        pugHealth = startingPugHealth;

        //distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void Update()
    {
        //update player transform
        try { 


                direction = player.position - transform.position;
                direction = player.position - transform.position;
                float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

                rb.rotation = angle;

                direction.Normalize();
                movement = direction;

                distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
                
        }
        catch { }

        
        
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
    }

    public void moveCharacter(Vector2 direction)
    {
        if (distanceToPlayer > 0.8f)
        {
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        }
    }

  


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            PugDies();
            
            
            //maybe make an action method for this
        }


    }

    public void PugDies()
    {
        
        pugHealth--;
        //call particle affect on pug (hearts and love)
        if (pugHealth == 0)
        {
            GameManager.instance.PugDestroyed(this);
            Destroy(this.gameObject);

        }

    }
}
 