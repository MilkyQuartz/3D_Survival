using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalIDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{

    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;
    public event Action OnTakeDamage;

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

    public void TakePhysicalIDamage(int damage)
    {
        health.Subtract(damage);
        OnTakeDamage?.Invoke();
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }

    public void IncreaseSize(float amount)
    {
        transform.localScale = new Vector3(amount, amount, amount);
    }
    public void ResetSizeAfterDelay(float waitTime)
    {
        StartCoroutine(ResetSize(waitTime));
    }
    IEnumerator ResetSize(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        transform.localScale = new Vector3(1, 1, 1);
    }
}
