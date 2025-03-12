using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using TMPro;

public class TPCC : MonoBehaviour
{
    public float punchSpeed = 1f;
    public float walkingSpeed = 2f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 60f;
    private bool isSprinting;
    private float rotation;
    private CharacterController characterController;
    public Animator animator;
    private bool punching;
    private bool invincible;
    private int punchFramesLeft = 0;
    public float punchDistance = 1.5f;
    public TextMeshProUGUI uiText;

    public Transform lookTarget;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        GetComponent<Rigidbody>().freezeRotation = true;
        uiText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Punch());
           
        }
        
    }

    IEnumerator Punch()
    {
        animator.SetBool("Punching", true);
        yield return new WaitForSeconds(1f); //Windup part of animation
        punching = true;
        // Create a ray from the player's position in the direction of the player's forward vector
        Ray ray = new Ray(transform.position + new Vector3(0, 1.15f, 0) + transform.forward*.25f, transform.forward);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * punchDistance, Color.red, 1f);
        // Cast the ray to detect a collision with an enemy
        if (Physics.Raycast(ray, out hit, punchDistance))
        {
            // Check if the ray hit an enemy object
            if (hit.collider != null)
            {
                // Print debug message for confirmation
                Debug.Log("Enemy hit: " + hit.collider.gameObject.tag);

                if (hit.collider.gameObject.tag == "Zombie")
                {
                    ZombieController zombie = hit.collider.gameObject.GetComponent<ZombieController>();
                    zombie.Die();
                }
                
            }
        }
        yield return new WaitForSeconds(.2f);//Actually throwing punch
        animator.SetBool("Punching", false);
        punching = false;
    }

    private void Move()
    {
        // Rotate with A-D
        float turnInput = Input.GetAxis("Horizontal");
        float turn = turnInput * rotationSpeed;
        float pace;
        transform.Rotate(0, turn * Time.deltaTime, 0);

        // Move with W-S
        // Sprint with Left Shift
        if (animator.GetBool("Punching"))
        {
             pace = punchSpeed;
            
        }
        else
        {
             pace = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkingSpeed;
        }
        float speed = pace * Input.GetAxis("Vertical");
        if (speed < -walkingSpeed) speed = -walkingSpeed;
        Vector3 movement = speed * transform.forward;
        characterController.SimpleMove(movement);
        
        animator.SetFloat("Speed", speed);
    }

    
    //adding look at position
    private void OnAnimatorIK()
    {
        animator.SetLookAtPosition(lookTarget.position);
        animator.SetLookAtWeight(1,0,1,0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!invincible &&  other.gameObject.CompareTag("Zombie") 
                        && !other.gameObject.GetComponent<ZombieController>().IsDead())
        {
            Destroy(gameObject);
            uiText.text = "You Lose...";
            uiText.enabled = true;
            
        }
    }

    public void SetInvincible(bool invincible)
    {
        this.invincible = invincible;
    }
}