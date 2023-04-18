using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ZoneData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SizeInTiles { get; set; }
    public Sprite Sprite { get; set; }
    public Sprite SeparatorSprite { get; set; }
}
public struct LevelData
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Sprite Background { get; set; }
    public Sprite SeparatorCommandBackIcon { get; set; }
    public Sprite SeparatorCommandBehindWallIcon { get; set; }
    public Sprite SeparatorCommandProtectWallIcon { get; set; }
    public Sprite SeparatorCommandForwardIcon { get; set; }

    public float GroundLevel { get; set; }
    public float DrowningInGround { get; set; }
    public ZoneData[] Zones { get; set; }
    public int ZonesCount { get; set; }
}
