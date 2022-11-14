using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Date")]
public class DungeonGenerationData : ScriptableObject
{
    public int numberofCrawlers;
    public int iterationMin;
    public int iterationMax;
}
