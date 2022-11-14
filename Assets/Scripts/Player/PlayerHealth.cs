using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [System.NonSerialized] public bool canTakeDamage = true;

    public HealthBar healthBar;

    HealthBarSystem healthBarSystem;
    [SerializeField] private SpriteRenderer spriteRenderer;


    //Instance
    public static PlayerHealth instance;

    //Void check
    public Transform voidCheck;
    public float voidCheckRadius;
    public LayerMask voidLayer;
    public LayerMask fallingBridgeLayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        healthBarSystem = new HealthBarSystem(PlayerMovement.instance.maxHp);
        for (int i = 0; i < PlayerMovement.instance.maxHp; i++)
        {
            healthBar.CreateHeartImage(new Vector2(40 * i - 900, 430)).SetHeart(true);
        }
    }

    /*public void Heal()
    {
        if (PlayerMovement.instance.currentHp + (PlayerMovement.instance.maxHp * 0.1) >= PlayerMovement.instance.maxHp)
        {
            PlayerMovement.instance.currentHp = PlayerMovement.instance.maxHp;
        }
        else
        {
            PlayerMovement.instance.currentHp += PlayerMovement.instance.maxHp / 10;
        }
        healthBar.SetHealth(PlayerMovement.instance.currentHp);
    }

    public bool TakeDamage(int _damage)
    {
        if (PlayerMovement.instance.currentHp - _damage <= 0)
        {
            PlayerMovement.instance.currentHp = 0;
            //healthBar.SetHealth(enemy.currentHealth);
            return true;
        }
        else
        {
            PlayerMovement.instance.currentHp -= _damage;
            //healthBar.SetHealth(enemy.currentHealth);
            return false;
        }
    }*/

    public void TakeDamage()
    {
        healthBarSystem.Damage();
        if (healthBar.Damage(healthBarSystem) == false)
        {
            GameOverManager.instance.GameOver();
        }
        StartCoroutine(InvincibilityTime());
        StartCoroutine(InvulnerabilityVisual());
    }

    #region invicibility
    IEnumerator InvincibilityTime()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1);
        canTakeDamage = true;
    }

    IEnumerator InvulnerabilityVisual()
    {
        while (canTakeDamage == false)
        {
            spriteRenderer.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region trigger
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWeapon") && canTakeDamage && PlayerMovement.instance.isDodging == false)
        {
            TakeDamage();
        }
        else if (collision.CompareTag("Void") && Physics2D.OverlapCircle(voidCheck.position, voidCheckRadius, voidLayer))
        {
            //RoomManager.instance.Respawn();
            TakeDamage();
        }
        else if (collision.CompareTag("FallingBridge") && Physics2D.OverlapCircle(voidCheck.position, voidCheckRadius, fallingBridgeLayer))
        {
            collision.GetComponent<FallingBridgeBlock>().Fall();
        }
    }
    #endregion
}