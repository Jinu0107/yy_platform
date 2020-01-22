using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int hp;
    public bool isEnemy;
    public ParticleSystem hurtEffect;

    private Animator anim;
    private int maxHP;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        maxHP = hp;
    }

    private void SetDamage(int value)
    {
        if (hp <= 0) return; //시체를 쏘는 것은 허용되지 않습니다.
        hp -= value;
        anim.SetTrigger("hurt");
        hurtEffect.Play();
        if(hp <= 0)
        {
            anim.SetBool("dead", true);
            anim.SetTrigger("deadTrigger");  //그래서 했구나...이해했다.
            
            if (isEnemy)
            {
                GetComponent<EnemyController>().SetDead();
            }else
            {
                //여기에는 플레이어의 SetDead 호출
            }
        }
    }

    public void SetDestroy()
    {
        gameObject.SetActive(false); //속도면에서 효율적이고
        //Destroy(gameObject); //저장공간 측면에서 효율적이야.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet" && collision.gameObject.activeSelf)
        {
            Bullet b = collision.gameObject.GetComponent<Bullet>();
            if (b && b.isEnemy != isEnemy)
            {
                b.SetDeactive();  //총알은 비활성화해서 재활용 가능하도록 해줌.
                SetDamage(b.damage);
            }
        }
    }

    public void AddHP(int value)
    {
        hp += value;
        if(hp > maxHP)
        {
            hp = maxHP;
        }
    }
}
