using UnityEngine;

public class Punch : MonoBehaviour
{
    [SerializeField]private float punchTime;
    private Rigidbody2D rb;
    private Vector2 direction;
    private float elapsedTime;
    
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    void Update(){
        if (elapsedTime > punchTime){
            Destroy(this.gameObject);
        }

        elapsedTime += Time.deltaTime;
    }

    public void punchLaunch(Vector2 direction)
    {
        this.direction = direction;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }
}
