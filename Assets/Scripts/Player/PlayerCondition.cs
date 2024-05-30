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
    Condition cold { get { return uiCondition.cold; } }

    public float noHungerHealthDecay;
    public float coldRecoveryRate;
    public float coldDecayRate;
    public event Action OnTakeDamage;
    public DayNightCycle dayNightCycle;
    void Start()
    {
        if (dayNightCycle == null)
        {
            dayNightCycle = FindObjectOfType<DayNightCycle>();
        }
    }

    void Update()
    {
        hunger.Subtract(hunger.regenRate * Time.deltaTime);
        stamina.Add(stamina.regenRate * Time.deltaTime);

        // 아침이면 추위 회복됨
        if (dayNightCycle.time >= 0.25f && dayNightCycle.time <= 0.75f)
        {
            cold.Add(coldRecoveryRate * Time.deltaTime);
        }
        // 밤이면 추위타서 깎이는데 0되면 체력에도 영향감
        else
        {
            cold.Subtract(coldDecayRate * Time.deltaTime);
        }

        if (hunger.curValue == 0f || cold.curValue == 0f)
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
    public void ColdHeal(float amount)
    {
        cold.Add(amount);
    }

    public void Eat(float amount) 
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("죽었다.");
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
    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
