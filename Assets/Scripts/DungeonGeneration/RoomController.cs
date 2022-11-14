using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int x;
    public int y;
}

public class RoomController : MonoBehaviour
{
    public static RoomController instance;

    string currentWorldName;

    RoomInfo currentLoadRoomData;

    Room currentRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    bool spawnedBossRoom = false;
    bool updatedRoom = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //LoadRoom("StartRoom", 0, 0);
        //LoadRoom("StartRoom", 1, 0);
        //LoadRoom("StartRoom", -1, 0);
        //LoadRoom("StartRoom", 0, 1);
        //LoadRoom("StartRoom", 0, -1);
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    //Vérifie si il y a des salles à charger, si oui, appelle la fonction pour les charger
    void UpdateRoomQueue()
    {
        if (isLoadingRoom)
        {
            return;
        }

        if (loadRoomQueue.Count == 0)
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRoom)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRoom = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if (loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.x, bossRoom.y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.x == tempRoom.x && r.y == tempRoom.y);
            loadedRooms.Remove(roomToRemove);
            //Créer la salle de boss
            LoadRoom("BossRoom", tempRoom.x, tempRoom.y);
        }
    }

    //Crée une nouvelle salle et la place dans la liste des salles à cherger
    public void LoadRoom(string _name, int _x, int _y)
    {
        if (DoesRoomExist(_x,_y))
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = _name;
        newRoomData.x = _x;
        newRoomData.y = _y;

        loadRoomQueue.Enqueue(newRoomData);
    }

    //Charge une salle de manière asynchrone
    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    //Lorsqu'une salle est chargée, elle appelle ce script afin de l'enregistrer dans liste des salles chargées
    public void RegisterRoom(Room room)
    {
        if (!DoesRoomExist(currentLoadRoomData.x, currentLoadRoomData.y))
        {
            room.transform.position = new Vector3
            (
                currentLoadRoomData.x * room.width,
                currentLoadRoomData.y * room.height,
                0
            );

            room.x = currentLoadRoomData.x;
            room.y = currentLoadRoomData.y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.x + ", " + room.y;
            room.transform.parent = transform;

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
                currentRoom = room;
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }

    //Recherche si une salle aux positions x et y existe
    public bool DoesRoomExist(int _x, int _y)
    {
        return loadedRooms.Find(item => item.x == _x && item.y == _y) != null;
    }

    public Room FindRoom(int _x, int _y)
    {
        return loadedRooms.Find(item => item.x == _x && item.y == _y);
    }

    //Fais changer de salle au joueur
    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;
    }
}
