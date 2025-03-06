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

    private Animator animator;

    private bool wandering;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if(Vector3.Distance(player.position, transform.position) <= chaseRadius){
                agent.SetDestination(player.position);
                UpdateSpeed(1f);
                wandering = false;
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
}
