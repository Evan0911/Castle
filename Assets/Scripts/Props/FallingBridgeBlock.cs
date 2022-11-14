using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBridgeBlock : MonoBehaviour
{
    private bool isFalling = false;

    [SerializeField] private Sprite voidSprite;

    public void Fall()
    {
        if (isFalling == false)
        {
            isFalling = true;
            StartCoroutine(FallTimer());
        }
    }

    IEnumerator FallTimer()
    {
        yield return new WaitForSeconds(0.06f);
        gameObject.tag = "Void";
        gameObject.layer = 6;
        gameObject.GetComponent<SpriteRenderer>().sprite = voidSprite;
    }
}
