using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEditor;

public class DataLoader : MonoBehaviour
{
    private string buildingsExample = $"Name = Barracks" +
        $"Image = Barracks.png" +
        $"Name = House" +
        $"Name = House.png";


    public List<GameObject> Buildings { get; private set; }
    public List<GameObject> Units { get; private set; }
    void Awake()
    {
        Buildings = new List<GameObject>();
        Units = new List<GameObject>();
        buildingParser(readFile("testXml"));
    }
    private string readFile(string fileName)
    {
        var textFile = Resources.Load<TextAsset>(fileName);
        Debug.Log(textFile.text);
        return textFile.text;
    }

    private void buildingParser(string xml)
    {
        XmlDocument xDoc = new XmlDocument();
        xDoc.LoadXml(xml);
        // получим корневой элемент
        XmlElement? xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            // обход всех узлов в корневом элементе
            foreach (XmlElement xnode in xRoot)
            {
                // получаем атрибут name
                XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                Debug.Log(attr?.Value);

                // обходим все дочерние узлы элемента user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // если узел - company
                    if (childnode.Name == "company")
                    {
                        Debug.Log($"Company: {childnode.InnerText}");
                    }
                    // если узел age
                    if (childnode.Name == "age")
                    {
                        Debug.Log($"Age: {childnode.InnerText}");
                    }
                }

            }
        }

    }
}
