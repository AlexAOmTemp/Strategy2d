using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
   
    private List<GameObject> _LevelList = new List<GameObject>();
    public GameObject CurrentLevel { get; private set; }
    public delegate void LevelCreated(GameObject Level);
    public static event LevelCreated LevelIsCreated;
    public GameObject CreateLevel(int levelId, bool reverse)
    {
        CurrentLevel = Instantiate(_levelPrefab, Vector3.zero, Quaternion.identity);
        CurrentLevel.GetComponent<LevelController>().Init (0, DataLoader.Levels[levelId],reverse,levelId);
        _LevelList.Add(CurrentLevel);
        LevelIsCreated?.Invoke(CurrentLevel);
        return CurrentLevel;
    }
}
