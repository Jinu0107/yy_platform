using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, Consumable
{
    public int hpValue = 6;
    public void consume(GameObject obj)
    {
        Health h = obj.GetComponent<Health>();
        h.AddHP(hpValue);
        gameObject.SetActive(false);
    }
}
