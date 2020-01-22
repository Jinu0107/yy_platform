using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isEnemy;
    public Vector2 dir;
    public float speed;
    public float timeToLive;
    public int damage;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rigid.velocity = dir * speed;    
    }
    
    IEnumerator CountTime()
    {
        yield return new WaitForSeconds(timeToLive);
        this.gameObject.SetActive(false);
    }

    public void SetDeactive()
    {
        StopCoroutine(CountTime());
        this.gameObject.SetActive(false);
    }

    public void SetBullet(Vector2 dir, float speed, int damage, 
        float timeToLive, bool isEnemy, Vector3 pos)
    {
        Vector3 scale = transform.localScale;
        scale.x = dir.x < 0 ? -1 : 1;
        transform.localScale = scale;

        this.dir = dir;
        this.speed = speed;
        this.damage = damage;
        this.timeToLive = timeToLive;
        this.isEnemy = isEnemy;
        this.transform.position = pos;

        StartCoroutine(CountTime());
    }
    
}
