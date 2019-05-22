using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public int health;
    int currentHealth;
    public Material playerMat; 

    // Start is called before the first frame update
    void Start()
    { 
        currentHealth = health;
        playerMat.SetFloat("_FadeAmount", 0.0f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(bool canTakeDamag, int damage)
    {
        if (canTakeDamag)
        {
            
            
            if (currentHealth > 0)
            {
               
                float amout = 1f - ((float)currentHealth / (float)health);
                currentHealth -= damage;
                playerMat.SetFloat("_FadeAmount", amout);
            }
            
            //Debug.Log(currentHealth);
        }
         
    }
}
