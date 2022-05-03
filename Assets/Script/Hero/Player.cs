using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;

    public Hp healthBar;  

    public int maxHealth = 100;
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth; 
        healthBar.SetMaxHealth(maxHealth);
        // animator.SetFloat("Hp",Mathf.Abs(currentHealth));                    Not in used
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if(currentHealth <= 0){
    //         Death();
    //     }
    // }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("waterball")){
            animator.SetTrigger("Damaged");
            TakeDamage(20);
        }

        if(currentHealth <= 0){
            Death();
        }
    }

    void TakeDamage (int damage)
    {
        currentHealth -= damage;
        // animator.SetFloat("Hp",Mathf.Abs(currentHealth));                   Not in used
        healthBar.SetHealth(currentHealth);
    }

    private void Death(){
        animator.SetTrigger("Death");
        rb.bodyType = RigidbodyType2D.Static;
    }

}
