using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifeTime;

    private BoxCollider2D boxColider;
    private Animator anim;

    private void Awake()
    {
        boxColider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hit) return;
        float movimentSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movimentSpeed, 0, 0);

        lifeTime += Time.deltaTime;
        if (lifeTime > 5) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxColider.enabled = false;
        anim.SetTrigger("explode");
    }
    public void setDirection(float _Direction)
    {
        lifeTime = 0;
        direction = _Direction;
        gameObject.SetActive(true);
        hit = false;
        boxColider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _Direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
