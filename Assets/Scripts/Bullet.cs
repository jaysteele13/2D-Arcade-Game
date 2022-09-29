using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 500.0f;
    public ParticleSystem explosion;

    public float maxLifetime = 10.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        rb.AddForce(-direction * this.speed);
        

        Destroy(this.gameObject, this.maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //this.explosion.transform.position = this.gameObject.transform.position;

        //var clone = gameObject;
        //clone = Instantiate(explosion, this.transform.position)

        this.explosion.Play();

        Destroy(this.gameObject);
       
    }
}
