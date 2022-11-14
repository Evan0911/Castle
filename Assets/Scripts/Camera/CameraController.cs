using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Room currentRoom;
    [SerializeField] public float moveSpeedWhenRoomChange;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    //Fais bouger la cam�ra si elle n'est pas � la bonne place (quand on change de salle)
    void UpdatePosition()
    {
        if (currentRoom == null)
        {
            return;
        }

        Vector3 targetPos = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    //Recup�re la position sur laquelle la cam�ra doit aller
    Vector3 GetCameraTargetPosition()
    {
        if (currentRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currentRoom.GetRoomCenter();
        targetPos.z = transform.position.z;
        return targetPos;
    }

    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
