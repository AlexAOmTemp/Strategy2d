using UnityEngine;
using System;

public class UnitBehavior : MonoBehaviour
{
    private PathPointsController _pathPointsController;
    private UnitController _unitController;
    public delegate void UnitInPoint();
    public event UnitInPoint UnitIsInPOint;
    private bool _isDone = false;
    private float _currentPathPoint;
    void Start()
    {
        var currentLevel = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().CurrentLevel;
        _unitController = this.GetComponent<UnitController>();
       // _currentPathPoint = PathPointsController.CurrentPathPoint;
    }
    void Update()
    {
        moveToPoint();
    }
    public void SetCurrentPathPoint(float pathPoint)
    {
        Debug.Log($"Path point reset to {pathPoint}");
        _currentPathPoint = pathPoint;
    }
    void moveToPoint()
    {
        if (Math.Abs(transform.position.x - _currentPathPoint) > 0.5)
        {
            //Debug.Log($"Unit move to point {_currentPathPoint}");
            _isDone = false;
            if (transform.position.x < _currentPathPoint)
                _unitController.CurrentState = UnitController.State.RunRight;
            else
                _unitController.CurrentState = UnitController.State.RunLeft;
        }
        else
        {
            //Debug.Log($"Unit has riched point {_currentPathPoint}");
            if (_isDone == false)
            {
                _isDone = true;
                _unitController.CurrentState = UnitController.State.Idle;
                UnitIsInPOint?.Invoke();
            }
        }
    }
}
