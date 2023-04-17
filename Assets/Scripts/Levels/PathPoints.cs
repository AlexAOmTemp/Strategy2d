using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPointsInitializer : MonoBehaviour
{
    public List<GameObject> PathPoints { get; private set; } = new List<GameObject>();
    public GameObject DefaultPoint { get; private set; }
    public int CreatePoint()
    {
        var point = new GameObject();
        point.transform.SetParent(this.transform);
        PathPoints.Add(point);
        if (PathPoints.Count >= 2)
            DefaultPoint = PathPoints[PathPoints.Count - 2]; //start of last zone
        return PathPoints.Count - 1;
    }
    public void ChangeDefaultPoint(int pointId)
    {
        DefaultPoint = PathPoints[pointId];
    }
}

