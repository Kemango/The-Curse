using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    Rigidbody2D rb;
    Transform groundCheck;
    public float speed;
    public Joystick mj;
    public Animator animator;
    public float jumpspeed;
    public CharacterController2D controller;
    public float jumpHeight = 2.5f;          // how high the jump reaches, in world units (tune this)
    public float gravityScale = 3f;          // higher = snappier, grounded feel (0.7 was very floaty)
    bool jump = false;
    bool jumpHeld = false;
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
        rb.gravityScale = gravityScale;            // force a snappy gravity regardless of the prefab value
        groundCheck = transform.Find("Bottom");    // the foot-point child the controller also uses

        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);
    }

    // Update is called once per frame
    private void Update()
    {
        // Drive the run animation from how hard the stick is pushed sideways
        animator.SetFloat("Speed", Mathf.Abs(mj.Horizontal * speed));

        // Jump on the rising edge of pushing up (must release before jumping again -> no auto bunny-hop)
        if (mj.Vertical > 0.5f)
        {
            if (!jumpHeld)
                jump = true;
            jumpHeld = true;
        }
        else if (mj.Vertical < 0.3f)
        {
            jumpHeld = false;
        }

        // Crouch only while the stick is clearly held down
        crouch = mj.Vertical < -0.6f;
    }

    // Keep the player inside the visible screen so they can't wander off the edge on any aspect ratio.
    void LateUpdate()
    {
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic)
            return;

        float halfW = cam.orthographicSize * cam.aspect - 0.5f;   // 0.5 ~= player half-width margin
        float camX = cam.transform.position.x;
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, camX - halfW, camX + halfW);
        transform.position = p;
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
        // Horizontal + crouch + facing are handled by the controller; we pass jump=false and do
        // the jump ourselves below so it doesn't depend on the controller's ground layer mask.
        controller.Move(mj.Horizontal * speed * 0.1f, crouch, false);

        // Reliable, layer-independent jump: if the stick was flicked up and we're standing on
        // something solid, launch straight up.
        if (jump && IsGrounded())
        {
            // Launch at exactly the speed needed to reach jumpHeight for a clean, predictable arc: v = sqrt(2*g*h)
            float g = Physics2D.gravity.magnitude * rb.gravityScale;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(2f * g * jumpHeight));
        }
        jump = false;
    }

    // Detects any solid (non-trigger) collider directly beneath the player's feet, regardless of
    // its layer. Robust against the ground-layer mask being misconfigured.
    bool IsGrounded()
    {
        if (groundCheck == null)
            return false;

        // Any solid (non-trigger) collider overlapping the feet counts as ground, on any layer.
        Collider2D[] cols = Physics2D.OverlapCircleAll(groundCheck.position, 0.25f);
        foreach (Collider2D c in cols)
        {
            if (c != null && !c.isTrigger && c.gameObject != gameObject)
                return true;
        }
        return false;
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