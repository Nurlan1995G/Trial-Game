using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 3f;
    public float jumpHeight = 6f;
    public Transform groundcheck;
    bool isGrounded;  
    Animator anim;
    int curHp;
    int maxHp = 3;
    bool isHit = false;   //в ударе


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        curHp = maxHp;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") == 0 && isGrounded)
            anim.SetInteger("State", 1);
        else 
        {
            Flip();
            if(isGrounded)
                anim.SetInteger("State", 2); 
        }
        CheckGround();

        FixedUpdate();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed,rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(transform.up * jumpHeight,ForceMode2D.Impulse);
    }

    public void RecountHp(int deltaHp)
    {
        curHp += deltaHp;
        if(deltaHp < 0)
        {
            isHit = true;
        }
        else if (curHp > maxHp)
        {
            curHp += deltaHp;
            curHp = maxHp;
        }
        if(curHp < 0)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            Invoke("Lose", 1.5f);
        }
    }

    private void Flip()
    {
        if (Input.GetAxis("Horizontal") > 0)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        if (Input.GetAxis("Horizontal") < 0)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
    }

    void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundcheck.position, 0.2f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded)
            anim.SetInteger("State", 3);
    }

    void Lose()
    {

    }
}
