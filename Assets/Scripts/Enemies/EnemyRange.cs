using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private CircleCollider2D rangeCollider;

    private void Start()
    {
        rangeCollider.radius = enemy.range;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.SetIsPlayerInRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            enemy.SetIsPlayerInRange(false);
        }
    }
}
