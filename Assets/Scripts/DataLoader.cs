using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEditor;

public struct BuildingParameters
{
    public string Name { get; set; }
    public string SpriteName { get; set; }

}
public class DataLoader : MonoBehaviour
{
   
    public static List<BuildingParameters> Buildings { get; private set; }
    public static List<GameObject> Units { get; private set; }
    void Awake()
    {
        Buildings = new List<BuildingParameters>();
        Units = new List<GameObject>();
        buildingParser(readFile("Buildings"));
    }
    private string readFile(string fileName)
    {
        var textFile = Resources.Load<TextAsset>(fileName);
        //Debug.Log(textFile.text);
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
                var buildingParameters = new BuildingParameters();
                
                XmlNode ? attr = xnode.Attributes.GetNamedItem("name");
                Debug.Log(attr?.Value);
                buildingParameters.Name = attr?.Value;

                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "sprite")
                        buildingParameters.SpriteName = childnode.InnerText;
                }
               
                Buildings.Add(buildingParameters);
                

            }
        }

    }
    
}
