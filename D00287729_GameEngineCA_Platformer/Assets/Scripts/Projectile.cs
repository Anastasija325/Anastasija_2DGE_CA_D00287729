using UnityEngine;

public class Projectile : MonoBehaviour
{

    private Rigidbody2D rb;
    private Vector2 direction;
    
    void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        this.direction = direction;
        rb.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);
    }
}
