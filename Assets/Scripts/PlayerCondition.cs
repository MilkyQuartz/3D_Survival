using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{

    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hunger.Subtract(hunger.regenRate * Time.deltaTime);
        stamina.Add(stamina.regenRate * Time.deltaTime);

        if(hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if(health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount) 
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("ав╬З╢ы.");
    }
}
