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

    private void Start()
    {
        CreateLevel(0);
    }
    public void CreateLevel(int levelId)
    {
        CurrentLevel = Instantiate(_levelPrefab, Vector3.zero, Quaternion.identity);
        CurrentLevel.GetComponent<LevelController>().Init(DataLoader.Levels[levelId]);
        _LevelList.Add(CurrentLevel);
        LevelIsCreated?.Invoke(CurrentLevel);
    }
}
