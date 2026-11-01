using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageNPC : MonoBehaviour
{
    private int health = 100;

    [Header("UI")]
    public Slider healthSlider; 

    void Start()
    {
        health = 100;
        UpdateHealthUI();
    }

    
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        UpdateHealthUI();

        if (health <= 0)
        {
            Die();
        }
    }


    public void gotHit()
    {
        TakeDamage(50);
    }

    
    public void gotHitByAutoGun()
    {
        TakeDamage(20);
    }

    
    public void gotHitByGrenade()
    {
        print("Hit by grenade");
        TakeDamage(100);
    }

    public void Die()
    {
        
        Destroy(gameObject);
    }

    void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = 100;
            healthSlider.value = health;
        }
    }
}
