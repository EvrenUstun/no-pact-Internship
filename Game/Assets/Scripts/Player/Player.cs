using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    //Rigidbody2D
    Rigidbody2D body2D;
    public float knockBackForce;

    //Colliderlar
    BoxCollider2D box2D;
    CircleCollider2D cir2D;

    ///  Bu var'lar karakterin hizini ve ziplama hizini belirler.
    [Tooltip("Karakterin ne kadar hizli gidecegini belirler")]
    [Range(0, 20)]
    public float playerSpeed;

    //Ziplama
    [Tooltip("Karakterin ne kadar yuksege ziplayacagini belirler")]
    [Range(0, 1500)]
    public float jumpPower;

    [Range(0, 50)]
    public float dashPower;

    internal bool canDash;
    internal bool canDamage;

    //Karekteri dondurme
    bool facingRight = true;

    // Yeri bulma
    [Tooltip("Karakterin yere degip deymedigini kontrol eder.")]
    public bool isGrounded;
    Transform groundCheck;
    const float GroundCheckRadius = 0.2f;
    [Tooltip("Yerin ne oldugunu belirler.")]
    public LayerMask groundLayer;

    //Animator Controller animasyonlari kontrol eder. 
    Animator playerAnimController;

    //Oyuncu cani. 
    internal int maxPlayerHealth = 100;
    public int currentPlayerHealth;
    internal bool isHurt;

    //Oyuncuyu oldur
    internal bool isDead;
    public float deadForce;

    //GameManager
    GameManager gameManager;

    public GameObject startPos;
  
    //Ates Etme 
    Transform firePoint;
    GameObject bullet;


    void Start()
    {
        transform.position = startPos.transform.position;
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

        //Animator u al
        playerAnimController = GetComponent<Animator>();

        // Cani max cana esitle
        currentPlayerHealth = maxPlayerHealth;

        //GamaManager
        gameManager = FindObjectOfType<GameManager>();

        //Ates etme 
        firePoint = transform.Find("FirePoint");
        bullet = Resources.Load("Bullet") as GameObject;
    }

    void Update()
    {
        UpdateAnimation();
        ReduceHealth();

        isDead = currentPlayerHealth <= 0;
        if (isDead)
            KillPlayer();

        //Eger canimiz maxCanimizdan yuksekse canýmýzý maxCana esitle.
        if (currentPlayerHealth > maxPlayerHealth)
        {
            currentPlayerHealth = maxPlayerHealth;
        }

        if (transform.position.y <= -20)
        {
            isDead = true;
        }
    }

    void FixedUpdate()
    {
        //yere degiyormuyuz diye bak
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);

        //Hareket etme
        //float h = Input.GetAxis("Horizontal");
        //body2D.velocity = new Vector2(h * playerSpeed, body2D.velocity.y);

        //Flip(h);

        if (isGrounded)
            canDamage = false;
    }

    public void Move(bool right)
    {
        if (right)
        {
            body2D.velocity = new Vector2(playerSpeed, body2D.velocity.y);
            Flip(1);
        }
        else
        {
            body2D.velocity = new Vector3(-playerSpeed, body2D.velocity.y);
            Flip(-1);
        }
    }

    public void ZeroVelocity()
    {
        body2D.velocity = Vector2.zero;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            //Rigidbody' ye dikey yonde (Y ypnunde) guc ekle
            body2D.AddForce(new Vector2(0, jumpPower));
            canDash = true;
        }
        else ForwardDash();
    }

    public void ForwardDash()
    {
        if (!isGrounded && canDash)
        {
            //Rigidbody' ye dikey yonde (Y ypnunde) ani bir guc ekler.
            body2D.AddForce(new Vector2(dashPower, 0), ForceMode2D.Impulse);
            canDamage = true;
            canDash = false;
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

    //Animator'u yenileme fonksiyonu
    void UpdateAnimation()
    {
        playerAnimController.SetFloat("VelocityX", Mathf.Abs(body2D.velocity.x));
        playerAnimController.SetBool("isGrounded", isGrounded);
        playerAnimController.SetFloat("VelocityY", body2D.velocity.y);
        playerAnimController.SetBool("isDead", isDead);
        if (isHurt && !isDead)
            playerAnimController.SetTrigger("isHurt");
    }

    //Can azaltma fonksiyonu
    void ReduceHealth()
    {
        if (isHurt)
        {
            //Eger canimiz              o zaman canimizdan zarar kadar cikar.
            //100 ise                   -zarar
            //eger bu kondisyon dogru ise can-zarar=yenican 
            isHurt = false;

            //eger havadaysak sol veya sag ve dikey yonde guc uygula
            if (facingRight && !isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 1000), ForceMode2D.Force);
            else if (!facingRight && !isGrounded)
                body2D.AddForce(new Vector2(knockBackForce, 1000), ForceMode2D.Force);

            //eger yerdeysek sol veya sag yonde guc uygula
            if (facingRight && isGrounded)
                body2D.AddForce(new Vector2(-knockBackForce, 0), ForceMode2D.Force);
            else if (!facingRight && isGrounded)
                body2D.AddForce(new Vector2(knockBackForce, 0), ForceMode2D.Force);
        }
    }

    public void ShootProjectile()
    {
        GameObject b = Instantiate(bullet) as GameObject; // Instantiate = mermi yaratýr.
        b.transform.position = firePoint.transform.position;
        b.transform.rotation = firePoint.transform.rotation;

        if (transform.localScale.x < 0)
        {
            b.GetComponent<Projectile>().bulletSpeed *= -1;
            b.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            b.GetComponent<Projectile>().bulletSpeed *= 1;
            b.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    //Oyuncuyu oldurme fonnksiyonu
    void KillPlayer()
    {
        isHurt = false;
        body2D.AddForce(new Vector2(0, deadForce), ForceMode2D.Impulse);
        body2D.drag += Time.deltaTime * 200;
        deadForce -= Time.deltaTime * 20;
        body2D.constraints = RigidbodyConstraints2D.FreezePositionX;
        box2D.enabled = false;
        cir2D.enabled = false;
    }
}
