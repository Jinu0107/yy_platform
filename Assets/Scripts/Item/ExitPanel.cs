using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{
    private bool cleared = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && cleared == false)
        {
            GameManager.instance.GoToNextStage();
            cleared = true;
        }
    }
}
