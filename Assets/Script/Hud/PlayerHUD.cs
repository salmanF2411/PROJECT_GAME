using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Wajib untuk akses UI

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
    public float staminaRegenRate = 15f; // Kecepatan isi ulang stamina per detik

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;

        //tur batas maksimal slider sesuai dengan maxHealth/maxStamina karakter kita
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
        // Jika stamina saat ini kurang dari batas maksimal, isi terus pelan-pelan
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            
            // Jangan biarkan stamina melebihi batas maksimal (100)
            if (currentStamina > maxStamina) 
            {
                currentStamina = maxStamina;
            }
            
            // Update posisi bar birunya agar bergerak naik
            if (staminaSlider != null)
            {
                staminaSlider.value = currentStamina;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; // Jangan sampai minus
        
        // Langsung update posisi bar merahnya
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0; // Jangan sampai minus
        
        // Update bar birunya saat dipakai
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }
}