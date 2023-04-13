using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitController : MonoBehaviour
{
    private UnitData _data;
    private State _currentState=State.Idle;
    private float _timeFromLastAttack;
    private GameObject _target;
    public enum State{
        Idle,
        MoveLeft,
        RunLeft,
        MoveRight,
        RunRight,
        MeleeAttack,
        SpellCast,
        RangeAttack,
        Build
    }
    public State GetState()
    {
        return _currentState;
    }
    public void Idle()
    {
        _currentState = State.Idle;
    }
    public void MoveLeft()
    {
        _currentState = State.MoveLeft;
    }
    public void RunLeft()
    {
        _currentState = State.RunLeft;
    }
    public void MoveRight()
    {
        _currentState = State.MoveRight;
    }
    public void RunRight()
    {
        _currentState = State.RunRight;
    }
    public bool TryMeleeAttack(GameObject target)
    {
        _target = target;
        _currentState = State.MeleeAttack;
        return true; 
    }
    public bool TryRangeAttack(GameObject target)
    {
        _target = target;
        _currentState = State.RangeAttack;
        return true;
    }
    public bool TryBuild(GameObject target)
    {
        _target = target;
        _currentState = State.Build;
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        _timeFromLastAttack += Time.deltaTime;
        switch (_currentState)
        {
            case State.MoveLeft: 
                transform.Translate(Vector3.left * _data.RunSpeed / 2 * Time.deltaTime);
                break;
            case State.RunLeft:
                transform.Translate(Vector3.left * _data.RunSpeed * Time.deltaTime);
                break;
            case State.MoveRight:
                transform.Translate(Vector3.right * _data.RunSpeed / 2 * Time.deltaTime);
                break;
            case State.RunRight:
                transform.Translate(Vector3.right * _data.RunSpeed * Time.deltaTime);
                break;
            case State.MeleeAttack:
                if (_timeFromLastAttack > 1 / _data.MeleeAttackSpeed)
                    meleeAttack();
                break;
            case State.SpellCast:
                break;
            case State.RangeAttack:
                break;
            case State.Build:
                break;
            case State.Idle:
            default:
                break;
        }
    }
    private void meleeAttack()
    {
        Debug.Log($"Unit {_data.Name}:{_data.Id} attacks {_target}");
    }
    private void rangeAttack()
    {
        Debug.Log($"Unit {_data.Name}:{_data.Id} attacks {_target}");
    }
}
