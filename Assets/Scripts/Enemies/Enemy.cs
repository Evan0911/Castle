using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Stats
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public float range;

    [SerializeField] private Animator animator;

    public EnemyHealth health;

    void Update()
    {
        Rotate();
        ChasePlayer();
    }

    virtual public void Rotate()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = PlayerMovement.instance.transform.position;
        Vector3 dir = playerPos - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    virtual public void ChasePlayer()
    {
        Vector3 pos = transform.position;
        Vector3 playerPos = PlayerMovement.instance.transform.position;
        Vector3 dir = playerPos - pos;
        transform.Translate(dir.normalized * Time.deltaTime, Space.World);
    }

    virtual public void SetIsPlayerInRange(bool isInRange)
    {
        animator.SetBool("isAttacking", isInRange);
    }

    virtual public void Death()
    {
        Destroy(gameObject);
    }    
}