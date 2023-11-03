using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public Animator animator;

    public int maxHealth = 3;
    int currentHealth;

    public LayerMask playerLayer;

    public Vector3 attackOffset;
    public float attackRate = 4f;
    float nextAttackTime = 0f;

    public float attackRange = 3f;
    public int attackDamage = 1;

    Transform player;
    Rigidbody2D rb;
    // Start is called at the start of program
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Vector2.Distance(player.position, rb.position) <= attackRange)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
    }

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, playerLayer);
        if (colInfo != null)
            colInfo.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        animator.SetTrigger("Hurt");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");

        animator.SetBool("IsDead", true);

        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {

        Gizmos.DrawWireSphere(transform.position + attackOffset, attackRange);
    }

}
