using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;
using System.Reflection;
using System.Globalization;

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
                object unitDataObj = buildingData;
                buildingData = (BuildingData)(loadParameters(unitDataObj, building.ChildNodes, _buildingsSpritesFolder));

                XmlNode attr = building.Attributes.GetNamedItem("name");
                buildingData.Name = attr?.Value;
                buildingData.TypeId = id++;

                List<Sprite> spr = new List<Sprite>();
                foreach (string name in buildingData.SpriteUnfinishedNames)
                    spr.Add(findSprite(_buildingsSpritesFolder, name));
                buildingData.SpritesUnfinished = spr.ToArray();
                buildingData.SpritesUnfinishedCount = spr.Count;
                if (buildingData.SpawnTypeName!=null)
                    buildingData.SpawnType = ParseEnum<SpawnTypes>(buildingData.SpawnTypeName);
                Buildings.Add(buildingData);
            }
            //consoleLogLoadedBuildingData();
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
            foreach (XmlElement level in xRoot)
            {
                int zoneId = 0;
                var levelData = new LevelData();
                List<ZoneData> zones = new List<ZoneData>();
                levelData.Id = LevelId;
                LevelId++;
                XmlNode attr = level.Attributes.GetNamedItem("name");
                levelData.Name = attr?.Value;
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
                    if (levelParameter.Name == "mainBuilding")
                    {
                        levelData.MainBuilding = levelParameter.InnerText;
                    }
                    if (levelParameter.Name == "commandBackIcon")
                    {
                        levelData.SeparatorCommandBackIcon = findSprite(_levelsSpritesFolder, levelParameter.InnerText);
                    }
                    if (levelParameter.Name == "commandBehindWallIcon")
                    {
                        levelData.SeparatorCommandBehindWallIcon = findSprite(_levelsSpritesFolder, levelParameter.InnerText);
                    }
                    if (levelParameter.Name == "commandProtectWallIcon")
                    {
                        levelData.SeparatorCommandProtectWallIcon = findSprite(_levelsSpritesFolder, levelParameter.InnerText);
                    }
                    if (levelParameter.Name == "commandForwardIcon")
                    {
                        levelData.SeparatorCommandForwardIcon = findSprite(_levelsSpritesFolder, levelParameter.InnerText);
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
                            if (zoneParameter.Name == "groundSprite")
                            {
                                foreach (var sprite in _dataFilesLoader.LoadedSpriteFiles[_levelsSpritesFolder])
                                    if (sprite.name == zoneParameter.InnerText)
                                    {
                                        zoneData.GroundSprite = sprite;
                                        break;
                                    }

                                if (zoneData.GroundSprite == null)
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
                            if (zoneParameter.Name == "zoneEndBuilding")
                                zoneData.EndBuilding = zoneParameter.InnerText;
                            if (zoneParameter.Name == "background")
                                zoneData.Background = findSprite(_levelsSpritesFolder, zoneParameter.InnerText);
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
            foreach (XmlElement unit in xRoot)
            {
                var unitData = new UnitData();
                object unitDataObj = unitData;
                unitData = (UnitData)(loadParameters(unitDataObj, unit.ChildNodes, _unitsSpritesFolder));
                unitData.TypeId = UnitTypeId;
                UnitTypeId++;
                XmlNode attr = unit.Attributes.GetNamedItem("name");
                unitData.Name = attr?.Value;
                Units.Add(unitData);
            }
            //consoleLogLoadedUnitData();
        }
    }

    private void consoleLogLoadedLevelData()
    {
        foreach (LevelData level in Levels)
        {
            Debug.Log($"Name = {level.Name} Id = {level.Id} GroundLevel = {level.GroundLevel} DrowningInGround = {level.DrowningInGround} Zones = {level.ZonesCount}");
            foreach (ZoneData zData in level.Zones)
                Debug.Log($"zoneData.id = {zData.Id} zoneData.name = {zData.Name} zoneData.size = {zData.SizeInTiles} zoneData.Sprite = {zData.GroundSprite} zoneData.SeparatorSprite  {zData.SeparatorSprite}");
        }
    }
    private void consoleLogLoadedData(List<object> dataSet)
    {
        Type myType = dataSet[0].GetType();
        FieldInfo[] fields = myType.GetFields();
        PropertyInfo[] Properties = myType.GetProperties();
        foreach (object data in dataSet)
        {
            string toLog = "";
            foreach (var field in fields)
                toLog += $"{field.Name} = {data.GetType().GetField(field.Name).GetValue(data)}; ";
            foreach (var property in Properties)
            {
                if (property.PropertyType.IsArray)
                {
                    string logString = $"{property.Name} = ";
                    Array array = (Array)property.GetValue(data);
                    if (array != null)
                        foreach (var value in array)
                            logString += $" {value} ";
                    logString += ";";
                    toLog += logString;
                }
                else
                    toLog += $"{property.Name} = {property.GetValue(data, null)}; ";
            }
            Debug.Log(toLog);
        }
    }
    private void consoleLogLoadedBuildingData()
    {
        List<object> objects = new List<object>();
        foreach (var unit in Buildings)
            objects.Add(unit);
        consoleLogLoadedData(objects);
    }
    private void consoleLogLoadedUnitData()
    {
        List<object> objects = new List<object>();
        foreach (var unit in Units)
            objects.Add(unit);
        consoleLogLoadedData(objects);
    }

    private Sprite findSprite(string folderName, string spriteName)
    {
        Sprite sprite = (_dataFilesLoader.LoadedSpriteFiles[folderName].Find(i => i.name.ToLower() == spriteName.ToLower()));
        if (sprite == null)
            Debug.LogError($"DataLoader: Sprite {folderName}/{spriteName} doesn't exist");
        return sprite;
    }
    private object loadParameters(object dataObj, XmlNodeList parameters, string spriteFolder)
    {
        Type myType = dataObj.GetType();
        List<PropertyInfo> Properties = new List<PropertyInfo>(myType.GetProperties());
        foreach (XmlNode parameter in parameters)
        {
            object val = null;
            var property = Properties.Find(i => i.Name == parameter.Name);
            if (property == null)
                Debug.Log($"There is no property {parameter.Name}");
            else
            {
                Type propertyType = property.PropertyType;
                if (propertyType == typeof(string))
                    val = parameter.InnerText;
                else if (propertyType == typeof(string[]))
                    val = parameter.InnerText.Split(" ");
                else if (propertyType == typeof(float?) || propertyType == typeof(float))
                {
                    string number = parameter.InnerText.Replace(",", ".");
                    val = float.Parse(number, CultureInfo.InvariantCulture);
                }
                else if (propertyType == typeof(int))
                    val = int.Parse(parameter.InnerText);
                else if (propertyType == typeof(int[]))
                {
                    List<int> ints = new List<int>();
                    string[] strings = parameter.InnerText.Split(" ");
                    foreach (string num in strings)
                        ints.Add(int.Parse(num));
                    val = ints.ToArray();
                }
                else if (propertyType == typeof(bool))
                    val = bool.Parse(parameter.InnerText);
                else if (propertyType == typeof(Sprite))
                    val = findSprite(spriteFolder, parameter.InnerText);
                else
                    Debug.Log($"No parser for type {propertyType}");
                //if (val != null)
                property.SetValue(dataObj, val);
            }
        }
        return dataObj;
    }
    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
