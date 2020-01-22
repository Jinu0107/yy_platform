using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    [Header("Player Property")]
    public float moveSpeed = 8f;
    public float jumpForce = 800f;
    public int maxJumpCount = 2;

    [Space(10)]
    [Header("Checkers")]
    public LayerMask whatIsGround;
    public Transform groundChecker;

    [Space(10)]
    [Header("weapon")]
    public int attackDamage;
    public float attackDelay;
    public float bulletSpeed;
    public float timeToLive;
    public Transform firePos;
    public Animator muzzleFlash;

    [Space(10)]
    [Header("Particles")]
    public ParticleSystem hitGround;

    private int jumpCount = 2;
    private bool isGround;
    private bool canShot = true; //현재 총알 발사 가능한 상태인지
    private Rigidbody2D rigid;
    private Animator anim;
    private BoxCollider2D boxCol;
    private CircleCollider2D circleCol;

    private float xMove; //좌우 움직임을 저장
    private float yMove; //상하 움직임을 저장
    private bool facingRight = true; //오른쪽을 보는가?
    private int currentJumpCount = 0; //현재 사용한 점프카운트
    private bool jumpKeyDown = false; //점프키가 눌렸는가?

    private bool onPlatform = false; //플랫폼 위에 있는지?
    private bool platformDown = false; //플랫폼에서 내려오고 있는지
    private bool isDead = false;  //죽었는지를 체크

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
        circleCol = GetComponent<CircleCollider2D>();
    }

    //캐릭터가 보고 있는 방향으로 스프라이트를 뒤집기
    private void Flip()  
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxisRaw("Vertical");
        
        if(xMove > 0 && !facingRight || xMove < 0 && facingRight)
        {
            Flip();
        }
        isGround = Physics2D.OverlapCircle(groundChecker.position, 0.2f, whatIsGround);
        if (Input.GetButtonDown("Jump") )
        {
            if(isGround || jumpCount > 0)
            {
                jumpKeyDown = true;
                jumpCount--;
            }
        }

        //속도가 -이면서 땅에 닿았다면 착지
        if(isGround && rigid.velocity.y < 0 && !platformDown)
        {
            jumpCount = maxJumpCount;
            boxCol.isTrigger = false;
            circleCol.isTrigger = false;
            hitGround.Play();
        }

        if(Input.GetButton("Fire1") && canShot)
        {
            FireWeapon();
            canShot = false;
            StartCoroutine(WaitDelay());
        }

        UpdateAnimator();
    }

    private void FireWeapon()
    {
        //실제 총알을 발사하는 부분
        Bullet bullet = BulletManager.instance.GetOrCreate();
        Vector3 dir = facingRight ? transform.right : transform.right * -1;
        bullet.SetBullet(dir, bulletSpeed, attackDamage, timeToLive, false, firePos.position);
        muzzleFlash.SetTrigger("fire");
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(attackDelay);
        canShot = true;
    }

    private void UpdateAnimator()
    {
        anim.SetFloat("xSpeed", Mathf.Abs(xMove));
        anim.SetFloat("ySpeed", rigid.velocity.y );
        anim.SetBool("isGround", isGround);
    }

    private void FixedUpdate()
    {
        if (isDead) return;
        rigid.velocity = new Vector2(xMove * moveSpeed, rigid.velocity.y);

        Collider2D groundCol =
            Physics2D.OverlapCircle(groundChecker.position, 0.2f, whatIsGround);

        if(groundCol && 
            groundCol.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            onPlatform = true;
        }else
        {
            onPlatform = false;
        }

        if (jumpKeyDown && yMove >= 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0);
            rigid.AddForce(new Vector2(0, jumpForce));
            jumpKeyDown = false;
        }else if(jumpKeyDown && yMove < 0 && onPlatform)
        {
            boxCol.isTrigger = true;
            circleCol.isTrigger = true;
            platformDown = true;
            jumpKeyDown = false;
            StartCoroutine(PlatformDownCount());
        }

        if(!isGround && rigid.velocity.y > 0.1)
        {
            boxCol.isTrigger = true;
            circleCol.isTrigger = true;
        }
    }

    IEnumerator PlatformDownCount()
    {
        yield return new WaitForSeconds(0.4f);
        platformDown = false;
    }

    public void SetDead()
    {
        isDead = true;
        xMove = 0;
        yMove = 0;
        boxCol.isTrigger = false;
        circleCol.isTrigger = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Consumable item = collision.gameObject.GetComponent<Consumable>();
        if(item != null)
        {
            item.consume(gameObject);
        }
    }

    public void ModJumpCnt(int value)
    {
        maxJumpCount += value;
        maxJumpCount = Mathf.Clamp(maxJumpCount, 2, 4);
        if (isGround)
        {
            currentJumpCount = maxJumpCount;
        }
    }
}
