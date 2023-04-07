using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuildingData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public float BuildingTime { get; set; }
    public Sprite[] SpritesUnfinished { get; set; }
    public int SpritesUnfinishedCount { get; set; }
    public Sprite SpriteFinished { get; set; }
    public int[] BuildInZones { get; set; }
    public bool CanBeBuild { get; set; }

}



