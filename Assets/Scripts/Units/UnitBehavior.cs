using UnityEngine;
using System;
using System.Collections.Generic;

public class UnitBehavior : MonoBehaviour
{
    [SerializeField] private TempCombat _tempCombat;
    private DistanceCollisionController _distanceController;
    private UnitController _unitController;
    public delegate void UnitInPoint();
    public event UnitInPoint UnitIsInPOint;
    private bool _isDone = false;
    private GameObject _currentPathPoint;
    private int _teamId;
    private List<GameObject> EnemiesAtMeleeDistance = new List<GameObject>();
    private List<GameObject> EnemiesAtRangeDistance = new List<GameObject>();
    private GameObject _target = null;
    private bool _targetInMelee;
    void Start()
    {
        //var currentLevel = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().CurrentLevel;
        _teamId = (int)this.GetComponent<Team>().Number;
        _unitController = this.GetComponent<UnitController>();
        _currentPathPoint = PathPointsController.GetCurrentPathPoint(_teamId);
        _distanceController = this.GetComponent<DistanceCollisionController>();
        Debug.Log($"Unit {this.gameObject.name} found DistCont{_distanceController}");
        _distanceController.MeleeDistance += onMeleeDistance;
        _distanceController.RangeDistance += onRangeDistance;
        _distanceController.MeleeDistanceLeft += onLeftMeleeDistance;
        _distanceController.MeleeDistanceLeft += onLeftRangeDistance;
    }
    bool first = true;
    void Update()
    {
        if (_target == null)
            moveToPoint();
        else
        {
            if (first)
            {
                Debug.Log ("attackState");
                first = false;
            }
            if (_targetInMelee == true)
                _unitController.CurrentState = UnitController.State.MeleeAttack;
            else
                _unitController.CurrentState = UnitController.State.RangeAttack;
        }
    }
    void moveToPoint()
    {
        if (Math.Abs(transform.position.x - _currentPathPoint.transform.position.x) > 0.5)
        {
            //Debug.Log($"Unit move to point {_currentPathPoint}");
            _isDone = false;
            if (transform.position.x < _currentPathPoint.transform.position.x)
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
    void onRangeDistance(GameObject target)
    {
        Debug.Log($"UnitBehavior: {target} range");
        if (target.GetComponent<Team>().Number != _teamId)
        {
            Debug.Log($"UnitBehavior: {target} is range target");
            EnemiesAtRangeDistance.Add(target);
            if (_target == null)
                setTarget(target, false);
        }
    }
    void onMeleeDistance(GameObject target)
    {
        Debug.Log($"UnitBehavior: {target} melee");
        if (target.GetComponent<Team>().Number != _teamId)
        {
            Debug.Log($"UnitBehavior: {target} is melee target");
            EnemiesAtMeleeDistance.Add(target);
            if (_target == null)
                setTarget(target, true);
        }
    }
    void onLeftRangeDistance(GameObject target)
    {
        if (EnemiesAtRangeDistance.Contains(target))
        {
            if (_target == target)
                searchNewTarget();
            EnemiesAtRangeDistance.Remove(target);
        }
    }
    void onLeftMeleeDistance(GameObject target)
    {
        if (EnemiesAtMeleeDistance.Contains(target))
        {
            if (_target == target)
                searchNewTarget();
            EnemiesAtMeleeDistance.Remove(target);
        }
    }
    void searchNewTarget()
    {
        if (EnemiesAtMeleeDistance.Count > 0)
            setTarget(EnemiesAtMeleeDistance[0],true);
        else if (EnemiesAtRangeDistance.Count > 0)
            setTarget(EnemiesAtRangeDistance[0],false);
        else
            _target = null;
    }
    void setTarget(GameObject target, bool melee)
    {
        _targetInMelee = melee;
        _target = target;
        _tempCombat.SetTarget(_target.GetComponent<TempCombat>());
    }
}
