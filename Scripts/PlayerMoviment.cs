using UnityEngine;

public class PlayerMoviment : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCoolDown;
    private float horizontalImput;
   

    private void Awake()
    {
        //pega os parametros do Rigidbody e Animator do objeto
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalImput = Input.GetAxis("Horizontal");

        //virar o personagem para esquerda e direita
        if(horizontalImput > 0.01f && body.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalImput < -0.01f && body.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //seta os parametros da animator
        anim.SetBool("run", horizontalImput != 0);
        anim.SetBool("grounded", isgrounded());

        //logica do wall jump
        if( wallJumpCoolDown > 0.2f)
        {
            body.velocity = new Vector2(horizontalImput * speed, body.velocity.y);

            if (onWall() && !isgrounded())
            {
                body.gravityScale = 2;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 3;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            wallJumpCoolDown += Time.deltaTime;
        }

    }


    private void Jump()
    {
        if (isgrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("Jump");
        }
        if(onWall() && !isgrounded())
        { 
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 15, 7);
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            wallJumpCoolDown = 0; 
            
        }
        
    }
    private bool isgrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center, 
            boxCollider.bounds.size, 
            0, Vector2.down, 
            0.01f, 
            groundLayer);

        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0, 
            new Vector2(transform.localScale.x, 0),
            0.01f,
            wallLayer);

        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalImput == 0 && isgrounded() && !onWall();
    }
}
