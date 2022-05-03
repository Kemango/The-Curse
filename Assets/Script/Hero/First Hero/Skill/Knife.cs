using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{

    public float speed;
    public int damage;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D (Collider2D hitInfo)
    {
        Water_Ghost_Script enemy = hitInfo.GetComponent<Water_Ghost_Script>();
        if(enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
