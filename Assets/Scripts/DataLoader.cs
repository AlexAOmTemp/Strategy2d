using System.Collections.Generic;
using UnityEngine;
using System.Xml;

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
                    zoneData.Name = zone.Attributes.GetNamedItem("name")?.Value;
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
                levelData.Zones = zones.ToArray();
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
            int id = 0;
            foreach (XmlElement building in xRoot)
            {
                var buildingData = new BuildingData();
                ConstructionData constructionData = new ConstructionData();

                XmlNode attr = building.Attributes.GetNamedItem("name");
                buildingData.Name = attr?.Value;
                buildingData.Id = id;
                id++;
                foreach (XmlNode buildingParameter in building.ChildNodes)
                {
                    
                    if (buildingParameter.Name == "sprite")
                    {
                        Sprite sprite = Resources.Load<Sprite>($"{_buildingsSpritesPath}{buildingParameter.InnerText}");
                        if (sprite == null)
                            Debug.LogError($"DataLoader: Sprite {_buildingsSpritesPath}{buildingParameter.InnerText} doesn't exist");
                        else
                            constructionData.SpriteFinished = sprite;
                    }
                    if (buildingParameter.Name == "buildTime")
                        constructionData.BuildingTime = float.Parse(buildingParameter.InnerText);
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
                                spr.Add(sprite);
                        }
                        constructionData.SpritesUnfinished = spr.ToArray();
                        constructionData.SpritesUnfinishedCount = spr.Count;
                    }
                    if (buildingParameter.Name == "buildZonesId")
                    {
                        List<int> zonesId = new List<int>();
                        string[] zones = buildingParameter.InnerText.Split(" ");
                        foreach (string num in zones)
                            zonesId.Add(int.Parse(num));
                        constructionData.BuildInZones = zonesId.ToArray();
                    }
                    if (buildingParameter.Name == "canBeBuild")
                    {
                        constructionData.CanBeBuild = bool.Parse(buildingParameter.InnerText);
                    }
                    if (buildingParameter.Name == "description")
                    {
                        constructionData.Description = buildingParameter.InnerText;
                    }
                }
                buildingData.ConstructionData = constructionData;
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
    private void consoleLogLoadedBuildingData()
    {
        foreach (BuildingData buildingData in Buildings)
        {
            Debug.Log($"Id = {buildingData.Id} Name = {buildingData.Name} BuildingTime = {buildingData.ConstructionData.BuildingTime} CanBeBuild = {buildingData.ConstructionData.CanBeBuild}");
            Debug.Log($"SpriteFinished = {buildingData.ConstructionData.SpriteFinished} SpritesUnfinishedCount = {buildingData.ConstructionData.SpritesUnfinishedCount}");
            foreach (Sprite sp in buildingData.ConstructionData.SpritesUnfinished)
                Debug.Log($"SpriteUnfinished {sp}");
            foreach (int zoneId in buildingData.ConstructionData.BuildInZones)
                Debug.Log($"BuildInZones = {zoneId}");
        }
    }
}
