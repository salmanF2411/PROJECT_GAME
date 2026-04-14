using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrajuritMove : MonoBehaviour
{
    public float speed = 5;

    public float batasKiri = -9;
    public float batasKanan = 9;

    void Update()
    {
        // gerak sesuai arah hadap object
        transform.position += transform.right * speed * Time.deltaTime;

        // cek batas kiri
        if (transform.position.x <= batasKiri && transform.right.x < 0)
        {
            Flip();
        }

        // cek batas kanan
        if (transform.position.x >= batasKanan && transform.right.x > 0)
        {
            Flip();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Flip();
    }

    void Flip()
    {
        // putar 180 derajat
        transform.Rotate(0, 180, 0);
    }
}