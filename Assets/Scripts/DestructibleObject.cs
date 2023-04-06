using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    private HealthData _baseData;
    private HealthData _currentMaximumData;
    private HealthData _currentData;


    public float GetCurrentHealth()
    {
        return _currentData.Health;
    }
    public float GetMaximumHealth()
    {
        return _currentData.Health;
    }



    public void Init(HealthData data)
    {
        _baseData = data;
        _currentMaximumData = _baseData;
        _currentData = _baseData;
    }
    public void ReceiveDamage(float damage)
    {
        float receivedDamage = 0;
        if (_currentData.Armor >= 0)
            receivedDamage = damage * (100 / (100 + _currentData.Armor));
        else
            receivedDamage = damage * (2 - (100 / (100 - _currentData.Armor)));
        _currentData.Health -= receivedDamage;
        if (_currentData.Health <= 0)
            Die();
    }
    public void Die()
    {
        Debug.Log("Object died");
        Destroy(this.gameObject);
    }
}
