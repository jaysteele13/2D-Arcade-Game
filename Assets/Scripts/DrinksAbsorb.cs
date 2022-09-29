using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinksAbsorb : MonoBehaviour
{
    //Script is a dupe of 'drinks' but for scripting to work it has to be like this
    public static DrinksAbsorb instance;

    public bool canAbsorb;
    public Sprite[] sprites;

    public float size = 1.0f, minSize = 0.5f, maxSize = 1.5f;

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
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

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
        if (collision.gameObject.tag == "Bullet")
        {
            if ((this.size / 2) > minSize)
            {
                CreateSplit();
                CreateSplit();
            }


            //Drinks Destroyed

            GameManager.instance.DrinkDestroyedAbsorb(this); //create bool to tell if drink is shot or absorbed - if shot no multipler if if not - multiplier!

            Destroy(this.gameObject);
        }

        if (this.canAbsorb && collision.gameObject.tag == "Player")
        {
            GameManager.instance.DrinkDestroyedAbsorb(this);
            Destroy(this.gameObject);
        }




    }

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        DrinksAbsorb half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;

        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speedOfDrinks);
    }


}



