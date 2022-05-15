using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private Rigidbody2D rb;

    public Hp healthBar;  
    public Mp manaBar;  

    public int maxHealth = 100;
    public int currentHealth;

    public int maxMana = 100;
    public int currentMana;

    [SerializeField]
	private GameObject gameOverUI;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth; 
        healthBar.SetMaxHealth(maxHealth);
        currentMana = maxMana; 
        manaBar.SetMaxMana(maxMana);

        animator.SetFloat("Hp",Mathf.Abs(currentHealth));
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if(currentHealth <= 0){
    //         Death();
    //     }
    // }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("waterball")){                //If hit by the ghost, u take 20 damaged
            animator.SetTrigger("Damaged");
            TakeDamage(20);
        }

        if(col.CompareTag("ghost")){                    //If touches the ghost, u take 20 damaged
            animator.SetTrigger("Damaged");
            TakeDamage(20);
        }

        if(currentHealth <= 0){
            Death();
            EndGame();
        }
    }

    void TakeDamage (int damage)
    {
        currentHealth -= damage;
        animator.SetFloat("Hp",Mathf.Abs(currentHealth));
        healthBar.SetHealth(currentHealth);
    }

    private void Death(){
        animator.SetTrigger("Death");
        rb.bodyType = RigidbodyType2D.Static;
    }

	public void EndGame ()
	{
		Debug.Log("GAME OVER");
		gameOverUI.SetActive(true);
	}
}
