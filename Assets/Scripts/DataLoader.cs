using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEditor;


public class DataLoader : MonoBehaviour
{
    public static List<BuildingData> Buildings { get; private set; }
    public static List<GameObject> Units { get; private set; }
    void Awake()
    {
        Buildings = new List<BuildingData>();
        Units = new List<GameObject>();
        buildingParser(readFile("Buildings"));
    }
    private string readFile(string fileName)
    {
        var textFile = Resources.Load<TextAsset>(fileName);
        return textFile.text;
    }

    private void buildingParser(string xml)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xml);
        XmlElement? xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            foreach (XmlElement xnode in xRoot)
            {
                var buildingParameters = new BuildingData();
                
                XmlNode ? attr = xnode.Attributes.GetNamedItem("name");
                Debug.Log(attr?.Value);
                buildingParameters.Name = attr?.Value;
                int id = 0;
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    buildingParameters.Id = id;
                    id++;
                    if (childnode.Name == "sprite")
                    {
                        Sprite sprite = Resources.Load<Sprite>($"Sprites/Buildings/{childnode.InnerText}");
                        if (sprite == null)
                            Debug.LogError($"DataLoader: Sprite Sprites/Buildings/{childnode.InnerText} doesn't exist");
                        else
                            buildingParameters.SpriteFinished = sprite;
                    }
                    if (childnode.Name == "buildTime")
                        buildingParameters.BuildingTime = float.Parse(childnode.InnerText);
                    if (childnode.Name == "spriteUnfinished")
                    {
                        List<Sprite> spr = new List<Sprite>();
                        string[] names = childnode.InnerText.Split(" ");
                        foreach (string name in names)
                        {
                           
                            Sprite sprite = Resources.Load<Sprite>($"Sprites/Buildings/{name}");
                            if (sprite == null)
                                Debug.LogError($"DataLoader: Sprite Sprites/Buildings/{name} doesn't exist");
                            else
                                spr.Add ( sprite);
                        }
                        buildingParameters.SpritesUnfinished = spr.ToArray();
                        buildingParameters.SpritesUnfinishedCount = spr.Count;
                        
                    }
                }             
                Buildings.Add(buildingParameters);
            }
        }
    }
}
