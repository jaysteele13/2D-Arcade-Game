using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PugHealthBar : MonoBehaviour
{
    public Image healthBar;
    private float currentHealth;
    private float maxHealth;
    public PugChasing pugScript;

    // Start is called before the first frame update
    void Start()
    {
        
        //pugScript = FindObjectOfType<PugChasing>();

        maxHealth = pugScript.pugHealth;


    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = pugScript.pugHealth;
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
