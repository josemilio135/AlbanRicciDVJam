using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float acceleration = 15f;
    public float deceleration = 15f;
    public float rotationSpeed = 10f;
    
    [Header("Model Reference")]
    public Transform modelTransform;
    
    [Header("Animation Parameters")]
    public Animator animator;
    
    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public LayerMask interactableLayer = -1;
    
    private Vector2 inputVector;
    private Vector2 currentVelocity;
    private float targetVelocity;
    private float smoothedVelocity;
    private bool isPicking;
    private bool isInteracting;
    private GameObject currentTarget;
    
    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (modelTransform == null)
        {
            modelTransform = transform.Find("Model");
            if (modelTransform == null)
            {
                Debug.LogWarning("Model Transform not found! Please assign the model child object to modelTransform field.");
            }
        }
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleInteraction();
        UpdateAnimationParameters();
    }
    
    void HandleInput()
    {
        inputVector = Vector2.zero;

        float xAxis = Input.GetAxisRaw("Horizontal");
        float yAxis = Input.GetAxisRaw("Vertical");
        inputVector = new Vector2(xAxis, yAxis).normalized;
    }
    
    void HandleMovement()
    {
        if (inputVector.magnitude > 0.01f)
        {
            targetVelocity = 1f;
            Vector3 moveDirection = new Vector3(inputVector.x, 0, inputVector.y);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            
            RotateTowardsDirection(moveDirection);
        }
        else
        {
            targetVelocity = 0f;
        }
        
        currentVelocity = Vector2.Lerp(currentVelocity, inputVector * targetVelocity, 
            (inputVector.magnitude > 0.01f ? acceleration : deceleration) * Time.deltaTime);
    }
    
    void RotateTowardsDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            if (modelTransform != null)
            {
                modelTransform.rotation = Quaternion.Slerp(modelTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                Debug.LogWarning("ModelTransform is null! Falling back to rotating the Player object itself.");
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
    
    void HandleInteraction()
    {
        DetectTarget();
        
        if (Input.GetKeyDown(KeyCode.F) || Input.GetMouseButtonDown(0))
        {
            if (currentTarget != null)
            {
                InteractableObject interactable = currentTarget.GetComponent<InteractableObject>();
                if (interactable != null)
                {
                    if (interactable.objectType == InteractableType.Pickup)
                    {
                        isPicking = true;
                        StartCoroutine(ResetPickingAfterDelay());
                    }
                    else
                    {
                        isInteracting = true;
                        StartCoroutine(ResetInteractingAfterDelay());
                    }
                }
            }
        }
    }
    
    void DetectTarget()
    {
        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        
        if (Physics.Raycast(rayOrigin, transform.forward, out hit, interactionRange, interactableLayer))
        {
            if (hit.collider.gameObject != currentTarget)
            {
                currentTarget = hit.collider.gameObject;
            }
        }
        else
        {
            currentTarget = null;
        }
    }
    
    void UpdateAnimationParameters()
    {
        if (animator != null)
        {
            smoothedVelocity = Mathf.Lerp(smoothedVelocity, targetVelocity, Time.deltaTime * 8f);
            animator.SetFloat("velocity", smoothedVelocity);
            animator.SetBool("isPicking", isPicking);
            animator.SetBool("isInteracting", isInteracting);
        }
    }
    
    IEnumerator ResetPickingAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isPicking = false;
    }
    
    IEnumerator ResetInteractingAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        isInteracting = false;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawRay(rayOrigin, transform.forward * interactionRange);
    }
}
