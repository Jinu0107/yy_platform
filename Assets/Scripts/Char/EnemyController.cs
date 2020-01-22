using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float patrolDistance; //패트롤할 거리
    public bool facingRight = true;

    public float speed = 10f;
    public Vector2 dir;

    public LayerMask enemyMask;
    public Animator muzzleFlash;

    [Header("Weapon")]
    public float shootingRange = 10f;
    public float shotDelay = 2.0f;
    public int attackDamage = 2;
    public float bulletSpeed = 20f;
    public float timeTolive = 2f;
    public Transform firePos;

    private Rigidbody2D rigid;
    private Animator anim;
    private bool canAttack = true; //발사가능여부
    private bool spotEnemy = false; //적 발견
    private bool isDead = false; //사망여부
    private float startPosition;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        startPosition = transform.position.x; //시작한 x좌표를 기억하고 있다.
        dir = transform.right; //처음 시작하면 오른쪽으로 움직인다.
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return; //죽은놈은 말이 없다.

        if(Mathf.Abs(transform.position.x - startPosition) >= patrolDistance)
        {
            dir *= -1;
            startPosition = transform.position.x;
        }
        if((facingRight && dir.x < 0) || (!facingRight && dir.x > 0))
        {
            Flip();
        }
        anim.SetFloat("xSpeed", Mathf.Abs(dir.x));

        Ray2D ray;
        if (facingRight)
        {
            ray = new Ray2D(transform.position, new Vector2(1, 0));
        }else
        {
            ray = new Ray2D(transform.position, new Vector2(-1, 0));
        }

        RaycastHit2D hit = 
            Physics2D.Raycast(ray.origin, ray.direction, shootingRange, enemyMask);

        if(hit.collider != null)
        {
            StopAndFire();
            spotEnemy = true;
        }else if(spotEnemy)
        {
            dir.x = facingRight ? 1 : -1;
            spotEnemy = false;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void StopAndFire()
    {
        if (canAttack)
        {
            canAttack = false;
            StartCoroutine(Attack());
        }

        if(dir.x != 0)
        {
            dir.x = 0;
        }
    }

    IEnumerator Attack()
    {
        muzzleFlash.SetTrigger("fire");
        Bullet temp = BulletManager.instance.GetOrCreate();
        Vector3 dir = facingRight ? transform.right : transform.right * -1;
        temp.SetBullet(dir, bulletSpeed, attackDamage, timeTolive, true, firePos.position);
        yield return new WaitForSeconds(shotDelay);
        canAttack = true;
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(dir.x * speed, rigid.velocity.y);
    }

    public void SetDead()
    {
        isDead = true;
        dir = Vector2.zero;
    }
}
