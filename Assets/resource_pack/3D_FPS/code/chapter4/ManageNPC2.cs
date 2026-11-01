using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManageNPC2 : MonoBehaviour
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
        print("Current Health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public void gotHit()
    {
        print("got hit by bullet");
        if(GetComponent<ControlNPCFSM>() != null)
            GetComponent<ControlNPCFSM>().setGotHitParameter();
        
        TakeDamage(50); 
    }

    public void gotHitByAutoGun()
    {
        print("got hit by auto gun");
        if(GetComponent<ControlNPCFSM>() != null)
            GetComponent<ControlNPCFSM>().setGotHitParameter();
        
        TakeDamage(20); 
    }

    public void gotHitByGrenade()
    {
        print("Hit by grenade");
        if(GetComponent<ControlNPCFSM>() != null)
            GetComponent<ControlNPCFSM>().setGotHitParameter();
        
        TakeDamage(100); 
    }

    public void Die()
    {
        print("NPC Died!");

        
        ControlNPCFSM fsm = GetComponent<ControlNPCFSM>();
        if (fsm != null)
        {
            
            fsm.setLowHealthParameter();
        }

        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        
        Destroy(gameObject, 2.0f);
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

