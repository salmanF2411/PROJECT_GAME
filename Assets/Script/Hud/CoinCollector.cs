using UnityEngine;
using TMPro; 

public class CoinCollector : MonoBehaviour
{
    [Header("Pengaturan UI")]
    public TextMeshProUGUI textKoin; 

    [Header("Pengaturan Koin")]
    [Tooltip("Tentukan berapa poin/koin yang didapat saat mengambil 1 item")]
    public int nilaiPerKoin = 1;

    private int jumlahKoin = 0;

    private void Start()
    {
        jumlahKoin = PlayerPrefs.GetInt("DataKoin", 0);
        
        // Perbarui tampilan teks saat mulai
        UpdateUITeks();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("koin"))
        {
            jumlahKoin += nilaiPerKoin; 
            
            PlayerPrefs.SetInt("DataKoin", jumlahKoin);
            PlayerPrefs.Save();

            // Perbarui tampilan teks di layar
            UpdateUITeks();

            Destroy(collision.gameObject);
        }
    }

    private void UpdateUITeks()
    {
        if (textKoin != null)
        {
            // Menggunakan simbol Dolar standar
            textKoin.text = "$ " + jumlahKoin.ToString();
        }
    }
}