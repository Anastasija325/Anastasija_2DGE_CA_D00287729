using UnityEngine;

public class PowerUp : MonoBehaviour
{

    [SerializeField]private Player player;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.toMechaTransform();
            Destroy(this.gameObject);
        }
    }
}
