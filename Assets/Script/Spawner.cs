using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
     // menampung apa yg mau kita spawn
    public GameObject objectEnemy;

    // lokasi enemy mau di spawn
    //Vector2 spawn = new Vector2(-1.05f, -0.623f); --> ini static

    // lakukan spawning
    void Spawning()
    {
        //1. apa yg mau di spawn
        //2. posisi dimana
        //3. rotasinya
        Instantiate(objectEnemy, transform.position, Quaternion.identity);

    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawning), 2f, 2f);
    }

}
