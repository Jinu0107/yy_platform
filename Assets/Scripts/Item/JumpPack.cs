using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPack : MonoBehaviour, Consumable
{
    public int value = 1;
    public void consume(GameObject obj)
    {
        PlayerScript p = obj.GetComponent<PlayerScript>();
        p.ModJumpCnt(value);
        gameObject.SetActive(false);
    }
}
