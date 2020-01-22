using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string nextScene;
    public Animator anim;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("게임 매니저가 2개 이상 있습니다.");
        }
        instance = this;
    }

    public void GoToNextStage()
    {
        StartCoroutine(ChangeLevel());
    }

    IEnumerator ChangeLevel()
    {
        anim.SetTrigger("Fadeout");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nextScene);
    }
}
