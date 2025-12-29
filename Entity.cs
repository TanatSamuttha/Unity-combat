using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public UnityEvent hitCheckEvent;
    public UnityEvent hitSuccessEvent;

    public float maxHP;
    public bool hitSuccess;
    public bool parrying;

    [SerializeField]
    private float currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public (float, float) GetHp()
    {
        return (currentHP, maxHP);
    }

    public (bool, bool) Hit(float damage)
    {
        hitCheckEvent.Invoke();
        bool stampHitSuccess = hitSuccess;
        bool stampParrying = parrying;
        hitSuccess = true;
        parrying = false;
        if (stampHitSuccess)
        {
            currentHP -= damage;
            hitSuccessEvent.Invoke();
            return (true, false);
        }
        else
        {
            return (stampHitSuccess, stampParrying);
        }
    }
}
