using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHUD : MonoBehaviour
{
    [Header("UI Referensi Slider")]
    public Slider healthSlider; 
    public Slider staminaSlider; 

    [Header("Status Player")]
    public float maxHealth = 100f;
    public float currentHealth;

    public float maxStamina = 100f;
    public float currentStamina;
    
    [Header("Pengaturan Regenerasi")]
    public float staminaRegenRate = 15f; 

    [Header("Pengaturan Makanan (Meal)")]
    [Tooltip("Berapa banyak darah yang bertambah saat memakan item dengan tag 'meal'")]
    public float nilaiPenyembuhan = 20f; 

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        // Atur batas maksimal slider sesuai dengan maxHealth/maxStamina karakter kita
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    private void Update()
    {
        // Tes tombol untuk mengurangi darah
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10f);
        }

        // --- REGENERASI STAMINA OTOMATIS ---
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            
            if (currentStamina > maxStamina) 
            {
                currentStamina = maxStamina;
            }
            
            if (staminaSlider != null)
            {
                staminaSlider.value = currentStamina;
            }
        }
    }

    // ==================================================
    // DETEKSI PENGAMBILAN ITEM (MEAL)
    // ==================================================
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("meal"))
        {
            if (currentHealth < maxHealth)
            {
                Heal(nilaiPenyembuhan);

                // Hancurkan objek makanan yang diambil agar hilang dari map
                Destroy(collision.gameObject);
            }
        }
    }

    // Fungsi khusus untuk menambah darah (Penyembuhan)
    public void Heal(float amount)
    {
        currentHealth += amount;
        
        if (currentHealth > maxHealth) 
        {
            currentHealth = maxHealth;
        }
        
        // Perbarui tampilan Slider darah secara instan
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; 
        
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0; 
        
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }
}