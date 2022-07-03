using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject realSword;
    public Rigidbody2D rb;
    public Collider2D coll,disColl;
    public LayerMask Ground;
    public Transform GroundCheck;
    public Animator anim;

    public float speed, jumpForce;
    private float horizontalmove;
    private bool isGround,isHurt,isAttacking;


    public float attackCoolDown,attackTime;
    private float attackTimeLeft, lastAttack;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (Time.time >= (lastAttack + attackCoolDown))
            {
                ReadyAttack();
            }
        }
        Attack();
        //isGround = Physics2D.OverlapCircle(GroundCheck.position, 0.7f, Ground);
        Movement();
        SwitchAnim();
    }

    void Movement()
    {
        anim.SetFloat("running", 0);
        float facedirection = Input.GetAxisRaw("Horizontal");
        horizontalmove = Input.GetAxis("Horizontal");
        if (horizontalmove != 0)
        {
            rb.velocity = new Vector3(horizontalmove * speed, rb.velocity.y, 0);
        }

        if (facedirection != 0)
        {
            transform.localScale = new Vector3(facedirection, 1, 1);
            anim.SetFloat("running", Mathf.Abs(facedirection));
        }
    }

    void Jump()
    {
        if (rb.IsTouchingLayers(Ground) && Input.GetKey(KeyCode.F))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void SwitchAnim()
    {
        anim.SetBool("idleing", false);
        if (rb.velocity.y < 0.1f && !coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurting", true);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurting", false);
                anim.SetBool("idleing", true);
                isHurt = false;
            }
        }
        else if (coll.IsTouchingLayers(Ground))
        {
            anim.SetBool("falling", false);
            anim.SetBool("idleing", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collection")
        {

        }
        if (collision.tag == "Deadline")
        {
            Invoke("Restart", 0.2f);
        }
        if (collision.tag == "DoubleJump")
        {

        }
        if (collision.tag == "Enter")
        {
           
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Damage")
        {
            rb.velocity = new Vector3(-50, 0, 0);
            isHurt = true;
        }
    }

    void Crouch()
    {
        if (Input.GetKey(KeyCode.C))
        {
                anim.SetBool("crouching", true);
                disColl.enabled = false;
        }
        else
        {
                anim.SetBool("crouching", false);
                disColl.enabled = true;
        }
    }

    void Attack()
    {
        if (isAttacking)
        {
            if (attackTimeLeft > 0)
            {
                realSword.SetActive(true);
                attackTimeLeft -= Time.fixedDeltaTime;

            }
            if (attackTimeLeft <= 0)
            {
                realSword.SetActive(false);
                isAttacking = false;
            }
        }
    }

    void ReadyAttack()
    {
        isAttacking = true;

        attackTimeLeft = attackTime;

        lastAttack = Time.time;
    }
}
