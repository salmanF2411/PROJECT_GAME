using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Pengaturan Gerak")]
    public float speed = 5;
    public Rigidbody2D rb;
    public int facingDirection = 1;
    public Animator animasi;

    [Header("Pengaturan Stamina")]
    public float staminaPakaiSaatJalan = 10f;  
    public float staminaPakaiSaatNabrak = 20f; 

    private PlayerHUD playerHUD; 

    void Start()
    {
        playerHUD = GetComponent<PlayerHUD>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 1. --- MENGURANGI STAMINA SAAT BERJALAN ---
        bool isMoving = Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0;

        if (isMoving && playerHUD != null)
        {
            playerHUD.UseStamina(staminaPakaiSaatJalan * Time.fixedDeltaTime);
        }

        
        // tidak bisa jalan saat staminanya mentok 0.
        // if (playerHUD != null && playerHUD.currentStamina <= 0)
        // {
        //     horizontal = 0;
        //     vertical = 0;
        // }

        if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
        {
            flip();
        }

        animasi.SetFloat("horizontal", Mathf.Abs(horizontal));
        animasi.SetFloat("vertical", Mathf.Abs(vertical));
        rb.velocity = new Vector2(horizontal, vertical) * speed;
    } 

    void flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // 2. --- MENGURANGI STAMINA SAAT MENABRAK ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Mengecek apakah benda yang ditabrak punya Tag bernama "objek"
        if (collision.gameObject.CompareTag("objek"))
        {
            if (playerHUD != null)
            {
                // Potong stamina secara langsung dalam jumlah besar
                playerHUD.UseStamina(staminaPakaiSaatNabrak);
            }
        }
    }
}