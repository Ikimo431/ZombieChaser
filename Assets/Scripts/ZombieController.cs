using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;


public class ZombieController : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;
    private float speed = 0;
    public float chaseRadius = 20f;
    public float attackRadius = 2f;

    private Animator animator;
    

    private bool wandering;
    private bool attacking;
    private bool dead;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player && !dead)
        {
            float distToPlayer = Vector3.Distance(player.position, transform.position);
            if (distToPlayer <= chaseRadius)
            {
                agent.SetDestination(player.position);
                UpdateSpeed(.85f);
                wandering = false;
                if (distToPlayer <= attackRadius && !attacking)
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                UpdateSpeed(0.5f);
                if (!wandering)
                {
                    StartCoroutine(Wander());
                }
            }
        }
        else {UpdateSpeed(0f);}
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            UpdateSpeed(0.5f);
        }
    } */

   IEnumerator Wander()
   {
       wandering = true;
       while (wandering)
       {
           agent.SetDestination(new Vector3(
               UnityEngine.Random.Range(transform.position.x-5, transform.position.x + 5),
               transform.position.y,
               UnityEngine.Random.Range(transform.position.z-5, transform.position.z + 5)
           ));
           yield return new WaitForSeconds(3); 
       }
           
   }
    private void UpdateSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
        animator.SetFloat("Speed", newSpeed);
        speed = newSpeed;
    }

   

    IEnumerator Attack()
    {
        animator.SetBool("Attacking", true);
        attacking = true;
        yield return new WaitForSeconds(2.1f);
        animator.SetBool("Attacking", false);
        attacking = false;
    }

    public void Die()
    {
        dead = true;
        UpdateSpeed(0f);
        animator.SetBool("Dead", true);
        animator.SetBool("Attacking", false);
        agent.enabled = false;
        

    }

    public bool IsDead()
    {
        return dead;
    }
}
