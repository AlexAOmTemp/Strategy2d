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
        // ������� �������� �������
        XmlElement? xRoot = xDoc.DocumentElement;
        if (xRoot != null)
        {
            // ����� ���� ����� � �������� ��������
            foreach (XmlElement xnode in xRoot)
            {
                // �������� ������� name
                XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                Debug.Log(attr?.Value);

                // ������� ��� �������� ���� �������� user
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // ���� ���� - company
                    if (childnode.Name == "company")
                    {
                        Debug.Log($"Company: {childnode.InnerText}");
                    }
                    // ���� ���� age
                    if (childnode.Name == "age")
                    {
                        Debug.Log($"Age: {childnode.InnerText}");
                    }
                }

            }
        }

    }
}
