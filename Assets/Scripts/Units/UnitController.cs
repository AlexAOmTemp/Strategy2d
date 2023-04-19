using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitController : MonoBehaviour
{
    private UnitData _data;
    public State CurrentState {get; set;}
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
    public void Init(UnitData data)
    {
        _data=data;
        Debug.Log("UnitController: unit initialized");
    }

    void Update()
    {
        _timeFromLastAttack += Time.deltaTime;
        switch (CurrentState)
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
        Debug.Log($"Unit {_data.Name}:{_data.TypeId} attacks {_target}");
    }
    private void rangeAttack()
    {
        Debug.Log($"Unit {_data.Name}:{_data.TypeId} attacks {_target}");
    }
}
