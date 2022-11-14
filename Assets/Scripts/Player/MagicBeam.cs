using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBeam : MonoBehaviour
{
    private Vector3 direction;

    private void Start()
    {
        StartCoroutine(DestroyCountdown());
    }

    void Update()
    {
        //Application du mouvement comme pour le joueur avec une direction normalisé pour convenir aux paramètres demandé et Space World pour un déplacement dans l'espace
        transform.Translate(direction.normalized * 5 * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    IEnumerator DestroyCountdown()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.transform.GetComponent<EnemyHealth>().TakeDamage(PlayerMovement.instance.GetAttack() / 2);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Switch"))
        {
            collision.GetComponent<Switch>().TurnOn();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Box") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
