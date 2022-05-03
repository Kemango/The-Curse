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

    public void Attack_btn ()        //Basic Attack Button
    {
        animator.SetTrigger("Attack");
        return;
    }

    public void Skill_btn ()        //Skill Button
    {
        if(currentMana>0){
        currentMana -= 10;
        manaBar.SetMana(currentMana);
        animator.SetFloat("Mp",Mathf.Abs(currentMana));
        animator.SetTrigger("Skill1");
        }
    
        if (currentMana <= 0){
            
        }
        
    }
    
    // void Fire ()
	// {

	// }


    private void FixedUpdate()
    {

        controller.Move(horizontalmove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}