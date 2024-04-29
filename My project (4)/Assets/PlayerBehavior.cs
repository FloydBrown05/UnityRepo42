using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerBehavior : MonoBehaviour
{

    public float moveSpeed = 10f;
    public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    public delegate void JumpingEvent();
    public event JumpingEvent playerJump;

    public GameObject bullet;
    public float bulletSpeed = 100f;
    
    private float _vInput;
    private float _hInput;
    private Rigidbody _rb;
    private CapsuleCollider _col;

    private GameBehavior _gameManager;

    public AudioSource audioPlayer;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _col = GetComponent<CapsuleCollider>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "CollisionTag")
        {
            audioPlayer.Play();
            Debug.Log("MadeASound");
        }
        
            {
            if(collision.gameObject.name == "Enemy")
            {
                _gameManager.HP -= 1;
            }

            if(collision.gameObject.name == "KelpJuice")
            {
                _gameManager.HP += 1;
            }

            if(collision.gameObject.name == "SeaShell")
            {
                _gameManager.HP -= 1;
            }
        }
    }
    void Update()
    {
        _vInput = UnityEngine.Input.GetAxis("Vertical") * moveSpeed;
        _hInput = UnityEngine.Input.GetAxis("Horizontal") * rotateSpeed;

        this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);
        this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);

    }

    void FixedUpdate()
    {
        Vector3 rotation = Vector3.up * _hInput;

        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position + this.transform.forward * _vInput * Time.fixedDeltaTime);

        _rb.MoveRotation(_rb.rotation * angleRot);

        if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
        }

        if (IsGrounded() && UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);

            playerJump();
        }

        if(UnityEngine.Input.GetMouseButtonDown(0))
        {
            GameObject newBullet = Instantiate(bullet, this.transform.position + new Vector3(1, 0, 0), this.transform.rotation) as GameObject;

            Rigidbody bulletRB = newBullet.GetComponent<Rigidbody>();

            bulletRB.velocity = this.transform.forward * bulletSpeed;
        }
    }

        private bool IsGrounded()
        {
            Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);

            bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore);
            return grounded;

        }

           
    }
