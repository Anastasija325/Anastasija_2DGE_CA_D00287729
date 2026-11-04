using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]private float walkTime;
    [SerializeField]private float idleTime;
    [SerializeField]private float fireTimer = 0.5f;
    [SerializeField]private GameObject projectile;
    [SerializeField]private Player player;

    private float elapsedTime;
    private int direction = 1;
    private bool isWalking = false;
    private Rigidbody2D rb;
    private Animator animator;
    private int stunCountdown = 3;
    private bool isStunned = false;
    private float invincibilityCount;
    private PolygonCollider2D polygonCollider;
    private float fireCountdown;
    private int HP = 3;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        animator.SetInteger("Direction",direction);
        animator.SetFloat("Move", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(isWalking && elapsedTime < walkTime)
        {
            Vector2 position = rb.transform.position;
            position.x += direction * Time.deltaTime;
            rb.transform.position = position;
        }
        else if(!isWalking && elapsedTime > idleTime)
        {
            isWalking = true;
            elapsedTime = 0;
            direction *= -1;
            animator.SetInteger("Direction", direction);
            animator.SetFloat("Move", direction);
        }
        else if(isWalking && elapsedTime > walkTime)
        {
            isWalking = false;
            elapsedTime = 0;  
            animator.SetFloat("Move", 0);
        }
        elapsedTime += Time.deltaTime;

        if (isStunned){
            invincibilityCount += Time.deltaTime;
            isWalking = false;
            fireCountdown = 0;
            if(invincibilityCount > 4)
            {
                isStunned = false;
                invincibilityCount = 0;
                polygonCollider.enabled = true; //turns collider on
                this.rb.gravityScale = 1.0f; //turns gravity on
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }
    }

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(rb.position.x, rb.position.y - 0.5f), new Vector2(direction, 0 ),
        5f, LayerMask.GetMask("Player"));
        if(hit.collider != null)
        {
            if(hit.collider.GetComponent<Player>() != null)
            {
                enemyFire();
            }
        }
        fireCountdown += Time.fixedDeltaTime;
    }

    private void enemyFire()
    {
        if (fireCountdown > fireTimer)
        {
            GameObject sb = Instantiate(projectile, new Vector2(rb.position.x, rb.position.y - 0.5f), Quaternion.identity);
            Projectile pro = sb.GetComponent<Projectile>();
            pro.Launch(new Vector2(direction, 0), 300);
            fireCountdown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Projectile>() != null)
        {
            rb.linearVelocity = new Vector2(0,0);
            hitGun();
        }
        if(collision.gameObject.GetComponent<Punch>() != null){
            rb.linearVelocity = new Vector2(0,0);
            isStunned = true;
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
            takeDmg();
        }
    }

    private void hitGun()
    {
        stunCountdown--;
        if(stunCountdown <= 0)
        {
            stunCountdown = 3;
            isStunned = true;
            this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
            polygonCollider.enabled = false; //turns collider off
            this.rb.gravityScale = 0.0f; //turns gravity off
            return;
        }
    }

    private void takeDmg(){
        HP--;
        if(HP <= 0){
            player.countKilled();
            Destroy(this.gameObject);
            return;
        }
    }
}
