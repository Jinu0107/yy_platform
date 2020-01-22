using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public Health playerHealth;
    private int maxHP;

    void Start()
    {
        maxHP = playerHealth.hp; //현재 HP를 최대 HP로 받아온다.
    }

    // Update is called once per frame
    void Update()
    {
        if( playerHealth != null)
        {
            int currentHP = playerHealth.hp;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            float ratio = (float)currentHP / (float)maxHP;
            transform.localScale = new Vector3(ratio, 1, 1);
        }
    }
}
