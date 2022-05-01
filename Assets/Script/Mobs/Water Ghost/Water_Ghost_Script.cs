using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Ghost_Script : MonoBehaviour
{

    public float speed; //How fast enemy moves
    public float stoppingDistance; // Higher the number the further the enemy will stop
    public float retreatDistance; // MOve away from player

    public Animator animator;

    private float timeBtwShots;
    public float startTimeBtwShots;
    
    public GameObject projectile;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBtwShots = startTimeBtwShots;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) > stoppingDistance){
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            float distance = Vector3.Distance(player.position, transform.position);
            animator.SetFloat("Speed",distance);
            animator.ResetTrigger("Attack"); 
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance && Vector2.Distance(transform.position, player.position) > stoppingDistance){
    //    else if (Vector2.Distance(transform.position, player.position) < stoppingDistance){
            transform.position = this.transform.position;
            float distance = Vector3.Distance(player.position, transform.position);
            animator.SetFloat("Speed",distance);
        }

        if(timeBtwShots <= 0 && Vector2.Distance(transform.position, player.position) < stoppingDistance){
            animator.SetTrigger("Attack");            
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        } else {
            timeBtwShots -= Time.deltaTime;
        }

        // if(timeBtwShots <= 0){
        //     Instantiate(projectile, transform.position, Quaternion.identity);
        //     timeBtwShots = startTimeBtwShots;
        // } else {
        //     timeBtwShots -= Time.deltaTime;
        // }

    }
}
