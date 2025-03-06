using Unity.VisualScripting;
using UnityEngine;

public class TPCC : MonoBehaviour
{
    public float walkingSpeed = 2f;
    public float sprintSpeed = 8f;
    public float rotationSpeed = 60f;
    private bool isSprinting;
    private float rotation;
    private CharacterController characterController;
    public Animator animator;

    public Transform lookTarget;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        // Rotate with A-D
        float turnInput = Input.GetAxis("Horizontal");
        float turn = turnInput * rotationSpeed;
        transform.Rotate(0, turn * Time.deltaTime, 0);

        // Move with W-S
        // Sprint with Left Shift
        float pace = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkingSpeed;
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
}