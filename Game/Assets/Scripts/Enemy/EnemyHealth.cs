using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxEnemyHealth;
    public float currentEnemyHealth;
    internal bool gotDamage;
    public float damage;
    public float projecttileDamage;
    Transform deathParticle;
    SpriteRenderer graph;
    CircleCollider2D cir2D;

    Player player;
    Rigidbody2D body2D;

    // Start is called before the first frame update
    void Start()
    {
        currentEnemyHealth = maxEnemyHealth;
        player = FindObjectOfType<Player>();
        graph = GetComponent<SpriteRenderer>();
        cir2D = GetComponent<CircleCollider2D>();
        body2D = GetComponent<Rigidbody2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEnemyHealth <= 0)
        {
            cir2D.enabled = false;
            graph.enabled = false;
           
            body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            Destroy(gameObject, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerItem" && player.canDamage)
        {
            currentEnemyHealth -= damage;
        }

        if (other.tag == "PlayerProjectile")
        {
            currentEnemyHealth -= projecttileDamage;
            Destroy(other.gameObject);
        }
    }
}
