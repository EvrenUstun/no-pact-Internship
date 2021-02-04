using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEnemyControl : MonoBehaviour
{

    Rigidbody2D enemyBody2D;
    public float enemySpeed;
    EnemyHealth enemyHealth;
    Animator monsterEnemyAnim;

    // duvarý bulma
    [Tooltip("Karakterin duvara degip deymedigini kontrol eder.")]
    bool isGrounded;
    Transform groundCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("Duvarýn ne oldugunu belirler.")]
    public LayerMask groundLayer;
    public bool moveRight;

    //Ucurum bulma 
    bool onEdge;
    Transform edgeCheck;

    // Start is called before the first frame update
    void Start()
    {
        enemyBody2D = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        edgeCheck = transform.Find("EdgeCheck");
        monsterEnemyAnim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void Update()
    {
            
        
        //duvara degiyormuyuz diye bak
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);
       
        onEdge = Physics2D.OverlapCircle(edgeCheck.position, GroundCheckRadius, groundLayer);

        if (isGrounded || !onEdge)
            moveRight = !moveRight;

        enemyBody2D.velocity = (moveRight) ? new Vector2(enemySpeed, 0) : new Vector2(-enemySpeed, 0);
        transform.localScale = (moveRight) ? new Vector2(-0.3533f, 0.3209f) : new Vector2(0.3533f, 0.3209f); 
    }
}
