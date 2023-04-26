using System.Collections.Generic;
using UnityEngine;

public class PathPointsController : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab;
    static public List<GameObject> PathPoints { get; private set; } = new List<GameObject>();
    static private List<GameObject> _currentPointMarker = new List<GameObject>();

    public void Start()
    {
        _currentPointMarker.Add(Instantiate(_markerPrefab, Vector3.zero, Quaternion.identity, this.transform));
        var enemyMarker = Instantiate(_markerPrefab, Vector3.zero, Quaternion.identity, this.transform);
        enemyMarker.GetComponent<SpriteRenderer>().color = new Color (1,1,1,0);

        _currentPointMarker.Add(enemyMarker);
    }
    static public void ChangeCurrentPathPoint(int teamId, int separatorId, int buttonId)
    {
        if (teamId >= _currentPointMarker.Count)
        {
            Debug.LogError($"PathPointsController: Bad Team Id {teamId}");
            return;
        }
        int pointId = separatorId * 2 + (buttonId - 1); //each separator have 2 points, button 0 is previous separator second point
        if (pointId >= 0 && pointId < PathPoints.Count)
        {
            _currentPointMarker[teamId].transform.position = new Vector3(
                PathPoints[pointId].transform.position.x,
                _currentPointMarker[teamId].transform.position.y, 0);
        }
        else
            Debug.LogError("PathPointsController: Attempting to change the current pathpoint to a nonexistent pathpoint");
    }
    static public GameObject GetCurrentPathPoint(int teamId)
    {
        if (teamId >= _currentPointMarker.Count)
        {
            Debug.LogError($"PathPointsController: Bad Team Id {teamId}");
            return null;
        }
        return _currentPointMarker[teamId].gameObject;
    }
}


