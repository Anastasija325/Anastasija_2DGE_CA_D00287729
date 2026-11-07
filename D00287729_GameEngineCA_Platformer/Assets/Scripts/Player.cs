using UnityEngine; 
using UnityEngine.InputSystem;
using NUnit.Framework;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //variables
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject punch;
    [SerializeField] private float JumpHeight = 2.4f;
    [SerializeField]private float shootTime;
    [SerializeField]private float punchTime;
    [SerializeField] private UIManager ui;
    [SerializeField]private int lives = 7;
    [SerializeField]private AudioClip punchSound;
    [SerializeField]private AudioClip MechaTransform;
    

    private Rigidbody2D rb;
    private Vector2 move;
    private int direction = 1;
    public float speed = 2.0f;
    private int jumpCount = 0;
    private bool atkAvailable = true;
    private Vector2 lastCheckpoint;
    private bool isMecha = false;
    private bool isPunch = false;
    private AudioSource audio;
    private int aliensRemaining = 13;

    //Animation
    private Animator animator;
    private float elapsedTime;
    
    //for invincibility
    private float invincibilityTime = 3.0f;
    private float invincibilityCount = 3.0f;
    public bool isInvin = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        lastCheckpoint = rb.transform.position; //records player start position
        
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if (!isPunch){
            move = InputSystem.actions["Move"].ReadValue<Vector2>();
        }
        
        if (move.x != 0)
        {
            direction = move.x < 0 ? -1 : 1;
        }

        //Jumps
        if (InputSystem.actions["Jump"].IsPressed() && jumpCount < 1){
            InputSystem.actions["Jump"].Reset();
            rb.linearVelocity = new Vector2(0,0);
            rb.AddForce(new Vector2(0, Mathf.Sqrt(-2*Physics.gravity.y*JumpHeight)), 
            ForceMode2D.Impulse);
            jumpCount++;
        }

        //checking for player invincibility
        if (isInvin)
        {
            invincibilityCount += Time.deltaTime;
            if(invincibilityCount > invincibilityTime)
            {
                isInvin = false;
                invincibilityCount = 0;
                this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            }
        }

        //Launching Projectiles
        if (InputSystem.actions["Attack"].IsPressed() && atkAvailable == true && !isMecha)
        {
            atkAvailable = false; //ensures player can't spam shoot
            elapsedTime = 0; //sets timer to 0 for the animation to play for 1 second
            animator.SetBool("isShoot", true); //plays shoot animation
            InputSystem.actions["Attack"].Reset(); //cancels any current attack so you can only so one per frame
            Fire(); //fire method
        
        }

        //Mecha Punch (Same as projectile attack, but activates the punchFire() method)
        if (InputSystem.actions["Attack"].IsPressed() && atkAvailable == true && isMecha)
        {
            atkAvailable = false;
            elapsedTime = 0;
            isPunch = true;
            move = new Vector2(0,0);
            audio.PlayOneShot(punchSound);
            animator.SetBool("isPunch", true);
            InputSystem.actions["Attack"].Reset();
            punchFire();
        
        }

         //Both for ensuring the animator plays full animation before allowing player to attack again
        if (elapsedTime > shootTime){
                animator.SetBool("isShoot", false);
                atkAvailable = true;
            }

        if (elapsedTime > punchTime){
            animator.SetBool("isPunch", false);
            isPunch = false;
            atkAvailable = true;
        }
        //timer
        elapsedTime += Time.deltaTime;

        //victory condition checker
        if (aliensRemaining == 0){
            move = new Vector2(0,0);
            ui.SetVictory();
        }
    }

    void FixedUpdate(){
        //Animation
        animator.SetInteger("Direction", direction);
        animator.SetFloat("Move", move.x);
        
        //Movement
        Vector2 position = rb.position;
        position.x += move.x * speed * Time.fixedDeltaTime;
        rb.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Ground"){
            jumpCount = 0; //checks whether player can jump again
        }
        
        if (collision.gameObject.GetComponent<Enemy>() != null && !isInvin && !isMecha){
            PlayerHit();
        }

        if(collision.gameObject.tag == "EnemyProjectile" && !isInvin){
            PlayerHit();
        }

        if(collision.gameObject.tag == "Respawn"){
            PlayerHit();
            rb.position = lastCheckpoint; //resets player position
        }

    }

    //taking damage
    private void PlayerHit()
    {
        isInvin = true; //sets invincibility
        this.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); //makes player transparent
        lives--;//lose life
        ui.SetLives(lives); //updates ui
        if (lives == 0)
        {
            SceneManager.LoadScene("SampleScene"); //death reloads the scene
        }
        
    }

    //player shoot stun gun
    private void Fire(){
        GameObject sb = Instantiate(projectile, rb.position, Quaternion.identity);  
        Projectile pro = sb.GetComponent<Projectile>();
        pro.Launch(new Vector2(direction, 0), 500);
    }

    //player punch as mecha
    private void punchFire(){
        GameObject sb;
        if(direction > 0){ //spawns a stationary invisible projectile in the direction player is facing
            sb = Instantiate(punch, new Vector2((rb.position.x + 2), rb.position.y), Quaternion.identity);
        }
        else{
            sb = Instantiate(punch, new Vector2((rb.position.x - 3), rb.position.y), Quaternion.identity);
        } 
        Punch pun = sb.GetComponent<Punch>();
        pun.punchLaunch(new Vector2(direction, 0));
    }
    //pick up mecha power up
    public void toMechaTransform(){
        isMecha = true;
        audio.PlayOneShot(MechaTransform);
        animator.SetBool("isMecha", true);
        this.rb.gravityScale = 2.0f; //change player gravity
    }

    public void countKilled(){ //counts how many enemies have been killed by the mecha
        aliensRemaining--;
        ui.SetScore(aliensRemaining);
    }
}
