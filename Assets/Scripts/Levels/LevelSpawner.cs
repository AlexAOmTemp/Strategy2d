using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
   
    private List<GameObject> _LevelList = new List<GameObject>();
    private GameObject _level;
    public delegate void LevelCreated(GameObject Level);
    public static event LevelCreated LevelIsCreated;

    private void Start()
    {
        CreateLevel(0);
    }
    void CreateLevel(int levelId)
    {
        _level = Instantiate(_levelPrefab, Vector3.zero, Quaternion.identity);
        _level.GetComponent<LevelController>().Init(DataLoader.Levels[levelId]);
        _LevelList.Add(_level);
        LevelIsCreated?.Invoke(_level);
    }
}
