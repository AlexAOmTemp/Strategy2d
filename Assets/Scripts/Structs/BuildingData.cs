using UnityEngine;

public enum SpawnTypes{
    Order,
    FreeSpawn
}
public struct BuildingData
{
    public int TypeId { get; set; }
    public string Name { get; set; }
    public string SpawnTypeName { get; set; }
    public SpawnTypes SpawnType { get; set; }
    public float BuildingTime { get; set; }
    public string[] SpriteUnfinishedNames { get; set; }
    public Sprite[] SpritesUnfinished { get; set; }
    public int SpritesUnfinishedCount { get; set; }
    public Sprite SpriteFinished { get; set; }
    public int[] BuildInZonesWithId { get; set; }
    public bool CanBeBuild { get; set; }
    public string Description { get; set; }
}



