using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Player: MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;
    private bool isGrounded = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatLayerMask;

    [HideInInspector] public bool isRight = true;

    private float checkRadius = 0.3f;

    private int score = 0;

    public delegate void RecountedScore(int score);
    public event RecountedScore OnRecountedScore;

    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

        OnRecountedScore?.Invoke(score);
    }

    private void Update()
    {
        Flip();
        Jump();
    }
    private void FixedUpdate()
    {
        CheckGround();
        rigidbody2D.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rigidbody2D.velocity.y );
            
    }
    public void ZeroPhysic()
    {
        rigidbody2D.velocity = Vector2.zero;
    }
    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            isRight = true;
        }
        else if(Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
            isRight = false;
        }
    }
    private void Jump()
    {
        if (isGrounded & Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    public void RecountScore(int score)
    {
        this.score += score;
        OnRecountedScore?.Invoke(this.score);
    }
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatLayerMask);
    }
}
