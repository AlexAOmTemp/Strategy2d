using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UnitBehavior : MonoBehaviour
{
    private PathPointsController _pathPointsController;
    private UnitController _unitController;
    public delegate void UnitInPoint();
    public event UnitInPoint UnitIsInPOint;
    private bool _isDone = false;
    void Start()
    {
        var currentLevel = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().CurrentLevel;
        _pathPointsController = currentLevel.GetComponent<PathPointsController>();
        _unitController = this.GetComponent<UnitController>();
    }
    void Update()
    {
        if (Math.Abs(transform.position.x - _pathPointsController.CurrentPathPoint) > 0.5)
        {
            _isDone = false;
            if (transform.position.x < _pathPointsController.CurrentPathPoint)
            {
                _unitController.CurrentState = UnitController.State.RunRight;
            }
            else if (transform.position.x > _pathPointsController.CurrentPathPoint)
            {
                _unitController.CurrentState = UnitController.State.RunLeft;
            }
        }
        else
        {
            if (_isDone == false)
            {
                _isDone = true;
                Debug.Log("Unit reach point");
                _unitController.CurrentState = UnitController.State.Idle;
                UnitIsInPOint?.Invoke();
            }
        }
    }
}
