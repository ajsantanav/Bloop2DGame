using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D player;
    private Animator anime;
    private BoxCollider2D boxCol;
    private float wallJumpCooldown;
    private float horizontalInput;

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;


    private void Awake()
    {
        //grab reference for components
        player = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flipp the player when moving to left or right
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //animator parameters
        anime.SetBool("run", horizontalInput != 0);
        anime.SetBool("ground", isGrounded());

        //wall jump logic
        if(wallJumpCooldown > 0.2f)
        {
            player.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, player.velocity.y);

            if (OnWall() && !isGrounded())
            {
                player.gravityScale = 0;
                player.velocity = Vector2.zero;
            }
            else
            {
                player.gravityScale = 1.5f;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
                /*
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
                    SoundManager.instance.PlaySound(jumpSound);
                */
            }

        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            player.velocity = new Vector2(player.velocity.x, jumpPower);
            anime.SetTrigger("Jump");
        }
        else if(OnWall() && !isGrounded())
        {
            if(horizontalInput == 0)
            {
                player.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3((transform.localScale.x), transform.localScale.y, transform.localScale.z);

            }
            else
            {
                player.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            }
            wallJumpCooldown = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
    }

    //checks if the player is grounded
    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    } 
    
    //checks if the player is on a wall
    private bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCol.bounds.center, boxCol.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}
