using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.Reflection;

public class DataLoader : MonoBehaviour
{
    [SerializeField] private DataFilesLoader _dataFilesLoader;
    private const string _unitsSpritesFolder = "Units";
    private const string _buildingsSpritesFolder = "Buildings";
    private const string _levelsSpritesFolder = "Levels";
    public static List<LevelData> Levels { get; private set; } = new List<LevelData>();
    public static List<BuildingData> Buildings { get; private set; } = new List<BuildingData>();
    public static List<UnitData> Units { get; private set; } = new List<UnitData>();


    void Awake()
    {
        levelParser(_dataFilesLoader.LoadedXMLFiles["Levels"]);
        buildingParser(_dataFilesLoader.LoadedXMLFiles["Buildings"]);
        unitParser(_dataFilesLoader.LoadedXMLFiles["Units"]);
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
                

                XmlNode attr = building.Attributes.GetNamedItem("name");
                buildingData.Name = attr?.Value;
                buildingData.TypeId = id;
                id++;
                foreach (XmlNode buildingParameter in building.ChildNodes)
                {
                    if (buildingParameter.Name == "sprite")
                    {
                        foreach (var sprite in _dataFilesLoader.LoadedSpriteFiles[_buildingsSpritesFolder])
                            if (sprite.name == buildingParameter.InnerText)
                            {
                                buildingData.SpriteFinished = sprite;
                                break;
                            }
                        if (buildingData.SpriteFinished == null)
                            Debug.LogError($"DataLoader: Sprite {_levelsSpritesFolder}{buildingParameter.InnerText} doesn't exist");
                    }
                    if (buildingParameter.Name == "buildTime")
                        buildingData.BuildingTime = float.Parse(buildingParameter.InnerText);
                    if (buildingParameter.Name == "spriteUnfinished")
                    {
                        List<Sprite> spr = new List<Sprite>();
                        string[] names = buildingParameter.InnerText.Split(" ");
                        foreach (string name in names)
                        {
                            Sprite sprite = (_dataFilesLoader.LoadedSpriteFiles[_buildingsSpritesFolder].Find(i => i.name == name));
                            if (sprite == null)
                                Debug.LogError($"DataLoader: Sprite {_buildingsSpritesFolder}{name} doesn't exist");
                            else
                                spr.Add(sprite);
                        }
                        buildingData.SpritesUnfinished = spr.ToArray();
                        buildingData.SpritesUnfinishedCount = spr.Count;
                    }
                    if (buildingParameter.Name == "buildZonesId")
                    {
                        List<int> zonesId = new List<int>();
                        string[] zones = buildingParameter.InnerText.Split(" ");
                        foreach (string num in zones)
                            zonesId.Add(int.Parse(num));
                        buildingData.BuildInZones = zonesId.ToArray();
                    }
                    if (buildingParameter.Name == "canBeBuild")
                    {
                        buildingData.CanBeBuild = bool.Parse(buildingParameter.InnerText);
                    }
                    if (buildingParameter.Name == "description")
                    {
                        buildingData.Description = buildingParameter.InnerText;
                    }
                }
                Buildings.Add(buildingData);
            }
        }
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
                foreach (XmlNode levelParameter in level.ChildNodes)
                {
                    if (levelParameter.Name == "groundLevel")
                    {
                        levelData.GroundLevel = float.Parse(levelParameter.InnerText);
                    }
                    if (levelParameter.Name == "drowningInGround")
                    {
                        levelData.DrowningInGround = float.Parse(levelParameter.InnerText);
                    }
                    if (levelParameter.Name == "background")
                    {
                        Sprite sprite = (_dataFilesLoader.LoadedSpriteFiles[_levelsSpritesFolder].Find(i => i.name == levelParameter.InnerText));
                        if (sprite == null)
                            Debug.LogError($"DataLoader: Sprite {_levelsSpritesFolder}{name} doesn't exist");
                        else
                        {
                            Texture2D texture = sprite.texture;
                            texture.wrapMode = TextureWrapMode.Repeat;
                            levelData.Background = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f, 0, SpriteMeshType.FullRect);
                        }
                    }
                    if (levelParameter.Name == "zone")
                    {
                        ZoneData zoneData = new ZoneData();
                        zoneData.Id = zoneId;
                        zoneId++;
                        zoneData.Name = levelParameter.Attributes.GetNamedItem("name")?.Value;
                        foreach (XmlNode zoneParameter in levelParameter.ChildNodes)
                        {
                            if (zoneParameter.Name == "sizeInTitles")
                            {
                                zoneData.SizeInTiles = int.Parse(zoneParameter.InnerText);
                                if (zoneData.SizeInTiles < 0 || zoneData.SizeInTiles > 1000)
                                    Debug.LogError($"DataLoader: levelParser incorrect zone {zoneData.Name}:{zoneData.Id} size value = {zoneData.SizeInTiles}");
                            }
                            if (zoneParameter.Name == "sprite")
                            {
                                foreach (var sprite in _dataFilesLoader.LoadedSpriteFiles[_levelsSpritesFolder])
                                    if (sprite.name == zoneParameter.InnerText)
                                    {
                                        zoneData.Sprite = sprite;
                                        break;
                                    }

                                if (zoneData.Sprite == null)
                                    Debug.LogError($"DataLoader: Sprite {_levelsSpritesFolder}{zoneParameter.InnerText} doesn't exist");
                            }
                            if (zoneParameter.Name == "separatorSprite")
                            {
                                Sprite sprite = (_dataFilesLoader.LoadedSpriteFiles[_levelsSpritesFolder].Find(i => i.name == zoneParameter.InnerText));
                                if (sprite == null)
                                    Debug.LogError($"DataLoader: Sprite {_levelsSpritesFolder}/{zoneParameter.InnerText} doesn't exist");
                                else
                                    zoneData.SeparatorSprite = sprite;
                            }
                        }
                        zones.Add(zoneData);
                    }

                }
                levelData.Zones = zones.ToArray();
                levelData.ZonesCount = zones.Count;
                Levels.Add(levelData);
            }
            //consoleLogLoadedLevelData();
        }
    }
    private void unitParser(string xml)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xml);
        XmlElement xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            int UnitTypeId = 0;
            foreach (XmlElement unit in xRoot) //level
            {
                var UnitData = new UnitData();
                UnitData.TypeId = UnitTypeId;
                UnitTypeId++;
                XmlNode attr = unit.Attributes.GetNamedItem("name");
                UnitData.Name = attr?.Value;

                foreach (XmlNode unitParameter in unit.ChildNodes)
                {

                    if (unitParameter.Name == "baseSpawningTime")
                    {
                        UnitData.BaseProductionTime = float.Parse(unitParameter.InnerText);
                    }
                    if (unitParameter.Name == "productsIn")
                    {
                        UnitData.ProductsIn = unitParameter.InnerText.Split(" ");
                    }
                    if (unitParameter.Name == "race")
                    {
                        UnitData.Race = unitParameter.InnerText;
                    }
                    if (unitParameter.Name == "runSpeed")
                    {
                        UnitData.RunSpeed = float.Parse(unitParameter.InnerText);
                    }

                    if (unitParameter.Name == "canFly")
                    {
                        UnitData.CanFly = bool.Parse(unitParameter.InnerText);
                    }
                    if (unitParameter.Name == "behaviorModel")
                    {
                        UnitData.BehaviorModel = Enum.Parse<UnitBehaviorModel>(unitParameter.InnerText);
                    }
                    if (unitParameter.Name == "constructionEfficiency")
                    {
                        UnitData.ConstructionEfficiency = float.Parse(unitParameter.InnerText);
                    }
                    if (unitParameter.Name == "spriteAlive")
                    {
                        Sprite sprite = (_dataFilesLoader.LoadedSpriteFiles[_unitsSpritesFolder].Find(i => i.name == unitParameter.InnerText));
                        if (sprite == null)
                            Debug.LogError($"DataLoader: Sprite {_unitsSpritesFolder}/{unitParameter.InnerText} doesn't exist");
                        else
                            UnitData.SpriteAlive = sprite;
                    }
                    if (unitParameter.Name == "spriteDead")
                    {
                        Sprite sprite = (_dataFilesLoader.LoadedSpriteFiles[_unitsSpritesFolder].Find(i => i.name == unitParameter.InnerText));
                        if (sprite == null)
                            Debug.LogError($"DataLoader: Sprite {_unitsSpritesFolder}/{unitParameter.InnerText} doesn't exist");
                        else
                            UnitData.SpriteDead = sprite;
                    }

                }
                Units.Add(UnitData);
            }
            consoleLogLoadedUnitData();
        }
    }
    private void consoleLogLoadedLevelData()
    {
        foreach (LevelData level in Levels)
        {
            Debug.Log($"Name = {level.Name} Id = {level.Id} GroundLevel = {level.GroundLevel} DrowningInGround = {level.DrowningInGround} Zones = {level.ZonesCount}");
            foreach (ZoneData zData in level.Zones)
                Debug.Log($"zoneData.id = {zData.Id} zoneData.name = {zData.Name} zoneData.size = {zData.SizeInTiles} zoneData.Sprite = {zData.Sprite} zoneData.SeparatorSprite  {zData.SeparatorSprite}");
        }
    }
    private void consoleLogLoadedBuildingData()
    {
        foreach (BuildingData buildingData in Buildings)
        {
            Debug.Log($"Id = {buildingData.TypeId} Name = {buildingData.Name} BuildingTime = {buildingData.BuildingTime} CanBeBuild = {buildingData.CanBeBuild}");
            Debug.Log($"SpriteFinished = {buildingData.SpriteFinished} SpritesUnfinishedCount = {buildingData.SpritesUnfinishedCount}");
            foreach (Sprite sp in buildingData.SpritesUnfinished)
                Debug.Log($"SpriteUnfinished {sp}");
            foreach (int zoneId in buildingData.BuildInZones)
                Debug.Log($"BuildInZones = {zoneId}");
        }
    }
    private void consoleLogLoadedUnitData()
    {
        Type myType = typeof(UnitData);
        FieldInfo[] fields = myType.GetFields();
        PropertyInfo[] Properties = myType.GetProperties();
        foreach (UnitData unitData in Units)
        {
            string toLog="";
            foreach (var field in fields)
            {
                toLog += $"{field.Name} = { unitData.GetType().GetField(field.Name).GetValue(unitData)}; ";
            }
            foreach (var property in Properties)
            {
                toLog += $"{property.Name} = { unitData.GetType().GetProperty(property.Name).GetValue(unitData,null)}; ";
            }
            Debug.Log(toLog);
        }
    }
}
