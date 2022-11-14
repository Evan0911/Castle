using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    private float widthOffset = 1.75f;

    public GameObject doorCollider;

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch(doorType)
            {
                case DoorType.bottom:
                    PlayerMovement.instance.transform.position = new Vector2(transform.position.x, transform.position.y - widthOffset);
                    break;
                case DoorType.top:
                    PlayerMovement.instance.transform.position = new Vector2(transform.position.x, transform.position.y + widthOffset);
                    break;
                case DoorType.left:
                    PlayerMovement.instance.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
                    break;
                case DoorType.right:
                    PlayerMovement.instance.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
                    break;
            }
        }
    }*/
}
