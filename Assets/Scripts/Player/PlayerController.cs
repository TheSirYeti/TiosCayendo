using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rb;
    Animator _animator;
    Camera cam;
    
    private void Awake()
    {
        cam = Camera.main;
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckJump();
        
        CheckCollisions();
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        DoWalking(h, v);
        
        FallingLogic();
        
        TimersManager();
    }

    [Header("WALK / RUN")]
    [SerializeField] float _walkingSpeed = 6f;
    [SerializeField] float _walkingLerpValue = 100f;

    void DoWalking(float h, float v)
    {
        if(isGrounded && !hasJumped) 
            _animator.SetFloat("WalkBlend", _rb.velocity.magnitude);
        
        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction == Vector3.zero)
        {
            return;
        }

        Vector3 forwardValue = cam.transform.forward.normalized;
        forwardValue.y = 0;
        
        Vector3 rightValue = cam.transform.right.normalized;
        forwardValue.y = 0;

        var targetVelocity = (forwardValue * v + rightValue * h) * _walkingSpeed + new Vector3(0, _rb.velocity.y, 0);
        transform.forward = forwardValue * v + rightValue * h; 
        
        _rb.velocity = Vector3.MoveTowards(_rb.velocity, targetVelocity, _walkingLerpValue * Time.deltaTime);
    }

    [Header("JUMP")]
    [SerializeField] private float jumpForce = 16f;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private bool hasJumped = false;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float jumpBuffer = 0.65f;
    [SerializeField] private float jumpFalloff = 8f;
    [SerializeField] private float coyoteTime = 0.3f;

    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _animator.SetTrigger("jump");
            jumpCooldown = jumpBuffer;
            
            _rb.velocity = new Vector3(_rb.velocity.x, jumpForce, _rb.velocity.z);
        }

        if (Input.GetKeyUp(KeyCode.Space) && _rb.velocity.y > 0)
        {
            _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y / 2f, _rb.velocity.z);
        }
    }


    [Header("FALL / GRAVITY")]
    [SerializeField] private float _fallValue = 7f;
    void FallingLogic()
    {
        if(_rb.velocity.y > 0)
            _rb.velocity += _fallValue * Physics.gravity.y * Vector3.up * Time.fixedDeltaTime;
    }

    [Header("COLLISIONS / RAYCAST")]
    [SerializeField] private float minFloorMultplier;
    [SerializeField] private LayerMask floorMask;
    
    void CheckCollisions()
    {
        if (Physics.Raycast(transform.position, Vector3.down * minFloorMultplier, minFloorMultplier, floorMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        
        _animator.SetBool("isGrounded", isGrounded);
        _animator.SetBool("hasJumped", hasJumped);
    }

    void TimersManager()
    {
        jumpCooldown -= Time.deltaTime;

        if (jumpCooldown < 0)
        {
            hasJumped = false;
        }
        else hasJumped = true;
    }
    
    private void OnDrawGizmos()
    {
        Vector3 dir = Vector3.down * minFloorMultplier;
        Gizmos.DrawRay(transform.position, dir);
        //Gizmos.DrawLine(transform.position + Vector3.down, transform.position + Vector3.down * 2);
    }
}