using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int health;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(bool canTakeDamag, int damage)
    {
        if (canTakeDamag)
        {
            currentHealth -= damage;
            //Debug.Log(currentHealth);
        }
         
    }
}
