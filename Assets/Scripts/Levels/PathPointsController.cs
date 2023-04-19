using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPointsController : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab;
    public List<float> PathPoints { get; private set; } = new List<float>();
    public float CurrentPathPoint { get; private set; }
    private GameObject _currentPointMarker;
    public void AddPoint(float point)
    {
        PathPoints.Add(point);
    }
    public void ChangeCurrentPathPoint(int separatorId, int buttonId)
    {
        int pointId = separatorId*2 + (buttonId - 1); //each separator have 2 points, button 0 is previous separator second point
        if (pointId>=0 && pointId<PathPoints.Count)
        {
            CurrentPathPoint = PathPoints[pointId];
            if (_currentPointMarker==null)
                 _currentPointMarker = Instantiate(_markerPrefab, Vector3.zero, Quaternion.identity, this.transform);
            _currentPointMarker.transform.position = new Vector3(CurrentPathPoint, _currentPointMarker.transform.position.y, 0);
        }
        else
            Debug.LogError("PathPointsController: Attempting to change the current pathpoint to a nonexistent pathpoint");
    }
}


