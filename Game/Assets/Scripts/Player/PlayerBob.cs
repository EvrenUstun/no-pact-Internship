using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBob : MonoBehaviour
{
    //Rigidbody2D
    Rigidbody2D body2D;

    //Colliderlar
    BoxCollider2D box2D;
    CircleCollider2D cir2D;

    ///  Bu var'lar karakterin hizini ve ziplama hizini belirler.
    [Tooltip("Karakterin ne kadar hizli gidecegini belirler")]
    public float playerSpeed;

    //Ziplama
    public float jumpPower;

    //Karekteri dondurme
    bool facingRight = true;

    // Yeri bulma
    [Tooltip("Karakterin yere degip deymedigini kontrol eder.")]
    public bool isGrounded;
    Transform groundCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("Yerin ne oldugunu belirler.")]
    public LayerMask groundLayer;

    //Ozel zýplama ozelliginin hizini belirler  
    [Tooltip("Karakterin ne kadar hizli yere inecegini belirler")]
    public float groundBreakerPower;

    internal bool canGroundBreaker;
    
    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody ayarlarý
        body2D = GetComponent<Rigidbody2D>();
        body2D.gravityScale = 5;
        body2D.freezeRotation = true;
        body2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        //Colliderlari al 
        box2D = GetComponent<BoxCollider2D>();
        cir2D = GetComponent<CircleCollider2D>();

        //GroundCheck i bul.
        groundCheck = transform.Find("GroundCheck");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //yere degiyormuyuz diye bak
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);

        //Hareket etme
        float h = Input.GetAxis("Horizontal");
        body2D.velocity = new Vector2(h * playerSpeed, body2D.velocity.y);

        Flip(h);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            //Rigidbody' ye dikey yonde (Y ypnunde) guc ekle
            body2D.AddForce(new Vector2(0, jumpPower));
            canGroundBreaker = true;
        } else GroundBreaker();
    }

    public void GroundBreaker()
    {
        if (!isGrounded && canGroundBreaker)
        {
            //Rigidbody' ye dikey yonde (Y ypnunde) ani bir guc ekler.
            body2D.AddForce(new Vector2(0 , -groundBreakerPower), ForceMode2D.Impulse);
            canGroundBreaker = false;
        }
    }

    //Karakteri dondurme fonksiyonu
    void Flip(float h)
    {
        if (h > 0 && !facingRight || h < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector2 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
       }
    }
}
