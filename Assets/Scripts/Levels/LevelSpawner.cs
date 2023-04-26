using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _levelPrefab;
    private List<GameObject> _LevelList = new List<GameObject>();
    public GameObject PlayerLevel { get; private set; }
    public delegate void LevelCreated(GameObject Level);
    public static event LevelCreated LevelIsCreated;
    public GameObject CreateLevel(int levelId, bool reverse, bool player)
    {
        var currentLevel = Instantiate(_levelPrefab, Vector3.zero, Quaternion.identity);
        currentLevel.GetComponent<LevelController>().Init (0, DataLoader.Levels[levelId],reverse,levelId,player);
        _LevelList.Add(currentLevel);
        LevelIsCreated?.Invoke(currentLevel);
        if (player == true)
            PlayerLevel= currentLevel;
        return currentLevel;

    }
}
