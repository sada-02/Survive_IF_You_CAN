using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 7; 
    public float lifetime = 5f; 
    public LayerMask obstacleMask; // reference to obstacle layer 

    private void Start()
    {
        Destroy(gameObject, lifetime);

        Collider2D bulletCollider = GetComponent<Collider2D>();
        Collider2D playerCollider = GameObject.FindGameObjectWithTag("player").GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(bulletCollider, playerCollider);
    }

    // if bullet hits an enemy, deal damage to it and destroy the bullet
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.GetComponent<EnemyBase>(); 
        if (enemy != null)
        {
            enemy.TakeDamage(damage); 
            Destroy(gameObject); 
            return;
        }

        if (((1 << collision.gameObject.layer) & obstacleMask) != 0) // destroy object if it hits an obstacle
        {
            Destroy(gameObject); 
            return;
        }
    }

}
