using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //public HealthBar healthBar;
    public Enemy enemy;

    private bool canTakeDamage = true;

    private void Start()
    {
        //healthBar.SetMaxHealth(enemy.maxHealth);
        //healthBar.SetHealth(enemy.maxHealth);
    }

    /*public int Heal()
    {
        if (enemy.currentHealth + (enemy.maxHealth * 0.1) >= enemy.maxHealth)
        {
            enemy.currentHealth = stats.maxHealth;
        }
        else
        {
            stats.currentHealth += stats.maxHealth / 10;
        }
        healthBar.SetHealth(stats.currentHealth);
    }*/

    public void TakeDamage(int _damage)
    {
        if (canTakeDamage)
        {
            if (enemy.currentHealth - _damage <= 0)
            {
                enemy.currentHealth = 0;
                enemy.Death();
                //healthBar.SetHealth(enemy.currentHealth);
            }
            else
            {
                enemy.currentHealth -= _damage;
                //healthBar.SetHealth(enemy.currentHealth);
            }
        }
    }

    #region invicibility
    IEnumerator invincibilityTime()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(0.5f);
        canTakeDamage = true;
    }
    #endregion

    #region trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            TakeDamage(PlayerMovement.instance.damage);
            StartCoroutine(invincibilityTime());
        }
    }
    #endregion
}
