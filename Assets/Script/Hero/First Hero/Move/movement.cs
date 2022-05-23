using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    public Joystick mj;
    public Animator animator;
    public float jumpspeed;
    public CharacterController2D controller;
    bool jump = false;
    float horizontalmove;
    float x;
    bool crouch = false;
    [Range(1, 10)]
    public float jumpvelocity;
    // Start is called before the first frame update

    public Mp manaBar;
    public int maxMana = 100;
    public int currentMana;

    public GameObject knifePrefab;
    public Transform firepoint;             //Skill

    public Transform attackPoint;            //Basic Attack
    public LayerMask enemyLayers;
    public float attackRange;
    public int damage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentMana = maxMana; 
        manaBar.SetMaxMana(maxMana);
    }

    // Update is called once per frame
    private void Update()
    {
        horizontalmove = x = mj.Horizontal * speed;

        animator.SetFloat("Speed",Mathf.Abs(horizontalmove));

        rb.velocity = new Vector2(x * speed, rb.velocity.y);
        float verticalmove = mj.Vertical * jumpspeed;
        if (verticalmove >= 7f)
        {
            jump = true;
            //GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpvelocity;   
        }
        if (verticalmove > -1f)
        {
            crouch = false;
        }
        else if (verticalmove < -4.5f)
        {
            crouch = true;
        }
        if (verticalmove >= 0f)
        {
            crouch = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Mana_Potions")){                    //If touches the ghost, u take 10 damaged
            Regen(20);
        }
    }

    void Regen (int MpRegen)
    {
        if(currentMana <= 80)
        {
            currentMana += MpRegen;
            animator.SetFloat("Mp",Mathf.Abs(currentMana));
            manaBar.SetMana(currentMana);  
        }
        else
        {
            currentMana = 100;
            manaBar.SetMana(currentMana);
        }

    }

    public void Attack_btn ()        //Basic Attack Button
    {
        Attack();
    }

    public void Skill_btn ()        //Skill Button
    {
        if(currentMana>0){
        currentMana -= 10;
        manaBar.SetMana(currentMana);
        animator.SetFloat("Mp",Mathf.Abs(currentMana));
        animator.SetTrigger("Skill1");
        Shoot_pro();
        }
    
        if (currentMana <= 0){
            
        }
        
    }
    
    // void Fire ()
	// {

	// }

    void Shoot_pro()
    {
        Instantiate(knifePrefab, firepoint.position, firepoint.rotation);
    }

    private void FixedUpdate()
    {

        controller.Move(horizontalmove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    void Attack()
    {
        //Play an attack animation
        animator.SetTrigger("Attack");

        //Detect enemy in range of attack
        Collider2D[] htiEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        
        foreach(Collider2D enemy in htiEnemies)
        {
            // enemy.GetComponent<Lightning_Ghost_Script>().TakeDamage(damage/2); 
            // enemy.GetComponent<Water_Ghost_Script>().TakeDamage(damage/2); 

            if(enemy.GetComponent<Lightning_Ghost_Script>() != null)
                {enemy.GetComponent<Lightning_Ghost_Script>().TakeDamage(damage);}
            else if(enemy.GetComponent<Water_Ghost_Script>() != null)
                {enemy.GetComponent<Water_Ghost_Script>().TakeDamage(damage);}
            else if(enemy.GetComponent<Fire_Ghost_Script>() != null)
                {enemy.GetComponent<Fire_Ghost_Script>().TakeDamage(damage);}
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}