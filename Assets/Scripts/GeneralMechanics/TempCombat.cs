using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCombat : MonoBehaviour
{
    private float _health = 100;
    private float _rangeDamage = 5;
    private float _meleeDamage = 5;
    private TempCombat _target;

    public void SetTarget(TempCombat target)
    {
        _target = target;
    }
    public void RangeAttack()
    {
        _target.GetDamage(_rangeDamage);
        Debug.Log($"Unit {this.gameObject.name} {_rangeDamage} range attack {_target.gameObject.name}");
    }
    public void MeleeAttack()
    {
       _target.GetDamage(_meleeDamage);
       Debug.Log($"Unit {this.gameObject.name} {_meleeDamage} melee attack {_target.gameObject.name}");
    }
    public void GetDamage(float damage)
    {
        this._health -= damage;
        Debug.Log($"Unit {this.gameObject.name} receive {damage} damage. HP {this._health}");
        if (this._health < 0)
        {
            Debug.Log($"Unit {this.gameObject.name} died");
            Destroy (this.gameObject);
        }
    }
}
