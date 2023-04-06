using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEditor;

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
    public ZoneData[] Zones { get; set; }
    public int ZonesCount { get; set; }


}


public class DataLoader : MonoBehaviour
{
    private const string _unitsSpritesPath = "Sprites/Units/";
    private const string _buildingsSpritesPath = "Sprites/Buildings/";
    private const string _levelsSpritesPath = "Sprites/Levels/";
    public static List<LevelData> Levels { get; private set; } = new List<LevelData>();
    public static List<BuildingData> Buildings { get; private set; } = new List<BuildingData>();
    public static List<GameObject> Units { get; private set; } = new List<GameObject>();
    void Awake()
    {
        levelParser(readFile("Levels"));

        buildingParser(readFile("Buildings"));

    }
    private string readFile(string fileName)
    {
        var textFile = Resources.Load<TextAsset>(fileName);
        return textFile.text;
    }
    private void levelParser(string xml)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xml);
        XmlElement xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            int LevelId = 0;
            foreach (XmlElement level in xRoot) //level
            {
                int zoneId = 0;
                var levelData = new LevelData();
                List<ZoneData> zones = new List<ZoneData>();            
                levelData.Id = LevelId;
                LevelId++;
                XmlNode attr = level.Attributes.GetNamedItem("name");
                levelData.Name = attr?.Value;
                Debug.Log($"levelData.Name = {levelData.Name}");
                foreach (XmlNode zone in level.ChildNodes)
                {
                    ZoneData zoneData = new ZoneData();
                    zoneData.Id = zoneId;
                    zoneId++;
                    zoneData.Name= zone.Attributes.GetNamedItem("name")?.Value;
                    foreach (XmlNode zoneParameter in zone.ChildNodes)
                    { 
                        if (zoneParameter.Name == "sizeInTitles")
                        {
                            zoneData.SizeInTiles = int.Parse(zoneParameter.InnerText);
                            if (zoneData.SizeInTiles < 0 || zoneData.SizeInTiles > 1000)
                                Debug.LogError($"DataLoader: levelParser incorrect zone {zoneData.Name}:{zoneData.Id} size value = {zoneData.SizeInTiles}");
                        }
                        if (zoneParameter.Name == "sprite")
                        {
                            Sprite sprite = Resources.Load<Sprite>($"{_levelsSpritesPath}{zoneParameter.InnerText}");
                            if (sprite == null)
                                Debug.LogError($"DataLoader: Sprite {_levelsSpritesPath}{zoneParameter.InnerText} doesn't exist");
                            else
                                zoneData.Sprite = sprite;
                        }
                        if (zoneParameter.Name == "separatorSprite")
                        {
                            Sprite sprite = Resources.Load<Sprite>($"{_levelsSpritesPath}{zoneParameter.InnerText}");
                            if (sprite == null)
                                Debug.LogError($"DataLoader: Sprite {_levelsSpritesPath}{zoneParameter.InnerText} doesn't exist");
                            else
                                zoneData.SeparatorSprite = sprite;
                        }
                    }
                    zones.Add(zoneData);
                }
                levelData.Zones= zones.ToArray();
                levelData.ZonesCount = zones.Count;
                Levels.Add(levelData);
            }
        }
    }
    private void buildingParser(string xml)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xml);
        XmlElement xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            foreach (XmlElement building in xRoot)
            {
                var buildingData = new BuildingData();
                
                XmlNode attr = building.Attributes.GetNamedItem("name");
                buildingData.Name = attr?.Value;
                int id = 0;
                foreach (XmlNode buildingParameter in building.ChildNodes)
                {
                    buildingData.Id = id;
                    id++;
                    if (buildingParameter.Name == "sprite")
                    {
                        Sprite sprite = Resources.Load<Sprite>($"{_buildingsSpritesPath}{buildingParameter.InnerText}");
                        if (sprite == null)
                            Debug.LogError($"DataLoader: Sprite {_buildingsSpritesPath}{buildingParameter.InnerText} doesn't exist");
                        else
                            buildingData.SpriteFinished = sprite;
                    }
                    if (buildingParameter.Name == "buildTime")
                        buildingData.BuildingTime = float.Parse(buildingParameter.InnerText);
                    if (buildingParameter.Name == "spriteUnfinished")
                    {
                        List<Sprite> spr = new List<Sprite>();
                        string[] names = buildingParameter.InnerText.Split(" ");
                        foreach (string name in names)
                        {
                           
                            Sprite sprite = Resources.Load<Sprite>($"{_buildingsSpritesPath}{name}");
                            if (sprite == null)
                                Debug.LogError($"DataLoader: Sprite {_buildingsSpritesPath}{name} doesn't exist");
                            else
                                spr.Add ( sprite);
                        }
                        buildingData.SpritesUnfinished = spr.ToArray();
                        buildingData.SpritesUnfinishedCount = spr.Count;
                        
                    }
                }             
                Buildings.Add(buildingData);
            }
        }
    }

    private void consoleLogLoadedLevelData()
    {
        foreach (LevelData level in Levels)
        {
            Debug.Log($"Name = {level.Name} Id = {level.Id} Zones = {level.ZonesCount}");
            foreach (ZoneData zData in level.Zones)
                Debug.Log($"zoneData.id = {zData.Id} zoneData.name = {zData.Name} zoneData.size = {zData.SizeInTiles} zoneData.Sprite = {zData.Sprite} zoneData.SeparatorSprite  {zData.SeparatorSprite}");
        }
    }
}
