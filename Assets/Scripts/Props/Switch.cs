using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    private bool state = false;
    [SerializeField] private bool isTemporary;

    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void TurnOn()
    {
        spriteRenderer.sprite = onSprite;
        state = true;
        if (isTemporary == true)
            StartCoroutine(TimeLimit());
        try
        {
            GetComponentInParent<SwitchGroup>().CheckStates();
        }
        catch
        {
            return;
        }
    }

    private void TurnOff()
    {
        if (isTemporary == true)
        {
            state = false;
            spriteRenderer.sprite = offSprite;
        }
    }

    public void Unactivate()
    {
        isTemporary = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            TurnOn();
        }
    }

    IEnumerator TimeLimit()
    {
        yield return new WaitForSeconds(1);
        TurnOff();
    }

    public bool GetState()
    {
        return state;
    }
}
