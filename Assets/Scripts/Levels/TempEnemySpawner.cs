using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    private List<UnitData> _unitDatas = new List<UnitData>();
    private List<float> _spawnCountdown = new List<float>();
    public void Awake()
    {
        foreach (UnitData data in DataLoader.Units)
            if (data.ProductsIn[0] == "EnemySpawner")
            {
                _spawnCountdown.Add(data.ProductionTime);
                _unitDatas.Add(data);
            }
    }
    bool createOne = false;
    void Update()
    {
        if (createOne == false)
        {
            for (int i = 0; i < _spawnCountdown.Count; i++)
            {
                _spawnCountdown[i] -= Time.deltaTime;
                if (_spawnCountdown[i] <= 0)
                {
                    createUnit(_unitDatas[i]);
                    _spawnCountdown[i] = _unitDatas[i].ProductionTime;
                    createOne = true;
                }
            }
        }
    }
    private void createUnit(UnitData data)
    {
        var unit = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity, this.transform);
        unit.transform.position = this.transform.position;
        unit.GetComponent<UnitMain>().Init(data);
        unit.GetComponent<UnitBehavior>().SetCurrentPathPoint(PathPointsController.PathPoints[0]);
    }
}
