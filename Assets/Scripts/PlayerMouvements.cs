using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvements: MonoBehaviour
{
    private Vector3 inputVector;
    private Animator animator;
    private bool grounded;
    [SerializeField]
    private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movements
        inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        animator.SetFloat("Speed", inputVector.z);
        animator.SetFloat("Turn", -inputVector.x);
        
        // Jump
        grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.1f, groundLayer);
        if (Input.GetButtonDown("Jump") && grounded) {
            Jump();
        }

        // Attack
        if (Input.GetButtonDown("Fire1")) {
            animator.SetTrigger("Slash");
        }
    }

    private void Jump() {
        animator.SetTrigger("Jump");
    }
}
