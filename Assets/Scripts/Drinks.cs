using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;


public class Drinks : MonoBehaviour
{
    public static Drinks instance;

    public bool canAbsorb;
    public Sprite[] sprites;

    public float size = 1.0f, minSize = 0.5f, maxSize =1.5f;

    public float speedOfDrinks = 6.0f;
    public float maxLifeTime = 15.0f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    
    private void Awake()
    {
        instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        try
        {
            spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        }
        catch(System.Exception e)
        {
            Debug.Log("Ignore this error (in drinks script): " + e);
        }
        

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        rb.mass = this.size;
    }


    public void SetTrajectory(Vector2 direction)
    {
        rb.AddForce(direction * this.speedOfDrinks);

        Destroy(this.gameObject, this.maxLifeTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if((this.size / 2) > minSize)
            {
                CreateSplit();
                CreateSplit();
            }


            //GameManager.instance.PugDestroyed(this);
            Destroy(this.gameObject);
        }

        if(this.canAbsorb && collision.gameObject.tag == "Player")
        {
            
            //GameManager.instance.PugDestroyed(this);
            Destroy(this.gameObject);
        }

        
        

    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Drinks half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;

        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speedOfDrinks);
    }




}

