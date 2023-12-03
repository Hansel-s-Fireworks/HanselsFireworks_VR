using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HPEvent : UnityEvent<int, int> { }

public class Status : MonoBehaviour
{
    //[HideInInspector]
    public HPEvent onHPEvent; //  = new HPEvent();

    [Header("HP")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int currentHP;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private void Awake()
    {
        if (onHPEvent == null) onHPEvent = new HPEvent();
        currentHP = maxHP;
    }
    public bool DecreaseHP(int damage)
    {
        int previousHP = currentHP;
        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(previousHP, currentHP);
        if (currentHP == 0) return true;
        return false;
    }

    public void IncreaseHP(int hp)
    {
        int previousHP = currentHP;
        currentHP = currentHP + hp > maxHP ? maxHP : currentHP + hp;
        onHPEvent.Invoke(previousHP, currentHP);
    }
}
