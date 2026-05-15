using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Ai_Agen_Enemy : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Return }

    [Header("Reference")]
    public Transform player;
    private SpriteRenderer spriteRenderer;
    private Vector2 homePosition;

    [Header("Radius Settings")]
    public float detectionRange = 5f;    
    public float stopChaseRange = 7f;     
    public float maxChaseDistance = 10f;  
    public float patrolRadius = 3f;

    [Header("Movement Settings")]
    public float speedPatroli = 2f;
    public float speedChase = 4f;
    public float patrolTime = 3f;
    
    [Header("Obstacle Avoidance")]
    public LayerMask obstacleLayer;      // Tentukan layer mana yang dianggap sebagai bangunan/tembok
    public float avoidDistance = 1.5f;   // Panjang sensor untuk mendeteksi tembok

    // --- TAMBAHAN KODE: PENGATURAN TEMPUR ---
    [Header("Combat Settings")]
    public float damage = 20f;           // Jumlah darah yang akan dikurangi
    public float bounceForce = 5f;       // Kekuatan pantulan saat menabrak player
    // ----------------------------------------
    
    private Vector2 direction;
    private Rigidbody2D rb;
    private float timer;
    private EnemyState currentState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        homePosition = transform.position; 
        
        currentState = EnemyState.Patrol;
        chooseRandom();
    }

    private void FixedUpdate()
    {
        float currentSpeed = (currentState == EnemyState.Chase) ? speedChase : speedPatroli;
        
        Vector2 finalDirection = AvoidObstacles(direction);
        
        rb.velocity = finalDirection * currentSpeed;
    }

    void Update()
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        float distToHome = Vector2.Distance(transform.position, homePosition);
        
        if (currentState != EnemyState.Chase)
        {
            // Cek jarak
            if (distToPlayer <= detectionRange && distToHome < maxChaseDistance)
            {
                Vector2 dirToPlayer = (player.position - transform.position).normalized;
                RaycastHit2D hitSight = Physics2D.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleLayer);
                
                if (hitSight.collider == null) 
                {
                    currentState = EnemyState.Chase; // Jika pandangan tidak terhalang, kejar!
                }
            }
        }
        else
        {
            if (distToPlayer > stopChaseRange || distToHome > maxChaseDistance + 1f)
            {
                currentState = EnemyState.Return;
            }
        }

        // --- EKSEKUSI PERGERAKAN ---
        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol(distToHome);
                break;
            case EnemyState.Chase:
                direction = (player.position - transform.position).normalized;
                break;
            case EnemyState.Return:
                HandleReturn(distToHome);
                break;
        }

        FlipSprite();
    }

    // MENGURANGI DARAH 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHUD playerHUD = collision.gameObject.GetComponent<PlayerHUD>();
            if (playerHUD != null)
            {
                playerHUD.TakeDamage(damage);
            }

            Vector2 bounceDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(bounceDirection * bounceForce, ForceMode2D.Impulse);
        }
    }
    // -------------------------------------------------------------

    Vector2 AvoidObstacles(Vector2 currentDir)
    {
        // Tembakkan sensor ke depan
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDir, avoidDistance, obstacleLayer);
        
        if (hit.collider != null)
        {
            // Jika menabrak, cari arah menyamping (tegak lurus) dari tembok agar bisa meluncur
            Vector2 normal = hit.normal;
            Vector2 slideDirection = new Vector2(-normal.y, normal.x); 

            // Pastikan arah meluncurnya searah dengan tujuan awal kita
            if (Vector2.Dot(slideDirection, currentDir) < 0)
            {
                slideDirection = -slideDirection;
            }

            // Gambar garis merah di jendela Scene sebagai penanda bahwa sensor mendeteksi halangan
            Debug.DrawRay(transform.position, slideDirection * avoidDistance, Color.red);
            return slideDirection.normalized;
        }

        // Gambar garis hijau jika jalanan aman
        Debug.DrawRay(transform.position, currentDir * avoidDistance, Color.green);
        return currentDir;
    }

    void HandlePatrol(float distToHome)
    {
        timer += Time.deltaTime;
        if (timer >= patrolTime)
        {
            chooseRandom();
            timer = 0f;
        }

        if (distToHome > patrolRadius)
        {
            direction = (homePosition - (Vector2)transform.position).normalized;
        }
    }

    void HandleReturn(float distToHome)
    {
        direction = (homePosition - (Vector2)transform.position).normalized;
        if (distToHome < 0.3f) 
        {
            currentState = EnemyState.Patrol;
            chooseRandom();
        }
    }

    void chooseRandom()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);
        direction = new Vector2(x, y).normalized;
    }

    void FlipSprite()
    {
        if (direction.x > 0.1f) spriteRenderer.flipX = false;
        else if (direction.x < -0.1f) spriteRenderer.flipX = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Application.isPlaying ? homePosition : (Vector2)transform.position, patrolRadius);

        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.white; 
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Application.isPlaying ? homePosition : (Vector2)transform.position, maxChaseDistance);
    }
}