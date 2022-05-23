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
        Water_Ghost_Script enemy1 = hitInfo.GetComponent<Water_Ghost_Script>();
        Lightning_Ghost_Script enemy2 = hitInfo.GetComponent<Lightning_Ghost_Script>();
        Fire_Ghost_Script enemy3 = hitInfo.GetComponent<Fire_Ghost_Script>();
        if(enemy1 != null)
        {
            enemy1.TakeDamage(damage);
        }
        if(enemy2 != null)
        {
            enemy2.TakeDamage(damage);
        }
        if(enemy3 != null)
        {
            enemy3.TakeDamage(damage);
        }      
        Destroy(gameObject);
    }
}
