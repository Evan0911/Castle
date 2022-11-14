using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData);
        SpawnRooms(dungeonRooms);
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("StartRoom", 0, 0);
        foreach(Vector2Int roomLocation in rooms)
        {
            //Changer le salle par défaut qui est générée
            RoomController.instance.LoadRoom("EmptyRoom", roomLocation.x, roomLocation.y);
        }
    }
}
