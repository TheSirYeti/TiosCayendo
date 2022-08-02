using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    Rigidbody _rb;
    public Rigidbody[] ragdollRigidbodies;
    public Collider[] ragdollColliders;
    Collider mainCollider;
    Animator _animator;
    Camera cam;

    [Header("PROPERTIES")] 
    [SerializeField] private Renderer renderer;
    [SerializeField] private Material enemyMat;
    [SerializeField] private CinemachineFreeLook freeLookCam;
    [SerializeField] private bool isInRagdollMode;
    [SerializeField] private float ragdollTime;
    
    private Transform currentCheckpoint;

    private bool canMove = false;
    
    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        
        EventManager.Subscribe("OnPlayerMovementChanged", SetMovementStatus);
        EventManager.Subscribe("OnRaceOver", SetMovementStatus);
        
        cam = Camera.main;
        mainCollider = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        if (!photonView.IsMine)
        {
            freeLookCam.enabled = false;
            renderer.material = enemyMat;
        }
    }

    private void Start()
    {
        StopRagdoll();
    }

    private void Update()
    {
        if (!photonView.IsMine || !canMove || isInRagdollMode) return;
        
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
    [SerializeField] private ParticleSystem jumpVFX;
    [SerializeField] private float jumpFalloff = 8f;
    [SerializeField] private float coyoteTime = 0.3f;

    void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            _animator.SetTrigger("jump");
            SoundManager.instance.PlaySound(SoundID.TIO_JUMP);
            jumpVFX.Play();
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
    [SerializeField] private LayerMask setParentMask;
    [SerializeField] private Transform nullParent;
    
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
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down * minFloorMultplier, out hit, minFloorMultplier, setParentMask))
        {
            photonView.RPC("RPC_SetParent", RpcTarget.AllBuffered, hit.collider.gameObject.name); //AAAAAAAAAAAA
        }
        else
        {
            photonView.RPC("RPC_SetParent", RpcTarget.AllBuffered, "null"); //AAAAAAAAAAAA2
        }
    }

    [PunRPC]
    void RPC_SetParent(string objectName)
    {
        if (objectName == "null")
        {
            transform.SetParent(null);
            return;
        }
        
        //Pido disculpas pero esto es una porqueria jasjasjas 
        Transform objectToFind = GameObject.Find(objectName).transform;
        transform.SetParent(objectToFind);
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

    void SetMovementStatus(object[] parameters)
    {
        bool status = (bool)parameters[0];
        photonView.RPC("RPC_AllowMovement", RpcTarget.AllBuffered, status);
    }

    [PunRPC]
    void RPC_AllowMovement(bool status)
    {
        canMove = status;
        
    }
    
    private void OnDrawGizmos()
    {
        Vector3 dir = Vector3.down * minFloorMultplier;
        Gizmos.DrawRay(transform.position, dir);
        //Gizmos.DrawLine(transform.position + Vector3.down, transform.position + Vector3.down * 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && photonView.IsMine)
        {
            StartCoroutine(DoRagdollCycle());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            currentCheckpoint = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Resetpoint"))
        {
            transform.position = currentCheckpoint.position;
        }
        
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && photonView.IsMine)
        {
            StartCoroutine(DoRagdollCycle());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            currentCheckpoint = other.transform;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Resetpoint"))
        {
            StopRagdoll();
            transform.position = currentCheckpoint.position;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new NotImplementedException();
    }

    [PunRPC]
    void RPC_DoRagdoll()
    {
        DoRagdoll();
    }
    
    [PunRPC]
    void RPC_StopRagdoll()
    {
        StopRagdoll();
    }
    
    public void DoRagdoll()
    {
        isInRagdollMode = true;
        foreach (var collider in ragdollColliders)
        {
            collider.enabled = true;
        }
        
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        mainCollider.enabled = false;
        _rb.isKinematic = true;
        _animator.enabled = false;
    }

    public void StopRagdoll()
    {
        isInRagdollMode = false;
        foreach (var collider in ragdollColliders)
        {
            collider.enabled = false;
        }
        
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        mainCollider.enabled = true;
        _rb.isKinematic = false;
        _animator.enabled = true;
    }

    IEnumerator DoRagdollCycle()
    {
        photonView.RPC("RPC_DoRagdoll", RpcTarget.AllBuffered);
        yield return new WaitForSeconds(ragdollTime);
        photonView.RPC("RPC_StopRagdoll", RpcTarget.AllBuffered);
    }
}
