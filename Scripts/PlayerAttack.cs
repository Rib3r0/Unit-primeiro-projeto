using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireBalls;

    private Animator anim;
    private PlayerMoviment playerMoviment;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMoviment = GetComponent<PlayerMoviment>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown && playerMoviment.canAttack())
        {
            attack();
        }
        cooldownTimer += Time.deltaTime;
    }

    private void attack()
    {
        anim.SetTrigger("Attack");
        cooldownTimer = 0;

        //poll fireball
        fireBalls[FindFireBall()].transform.position = firePoint.position;
        fireBalls[FindFireBall()].GetComponent<Projectile>().setDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireBall()
    {
        for(int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

}
