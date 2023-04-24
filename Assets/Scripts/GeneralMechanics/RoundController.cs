using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    private List<GameObject> _levels;
    void Start()
    {
        var level1 =_levelSpawner.CreateLevel(0,false);
        var level2 =_levelSpawner.CreateLevel(1,true);
        level2.transform.Translate (Vector3.right*200);
        var level1Controller = level1.GetComponent<LevelController>();
        var level2Controller = level2.GetComponent<LevelController>();
        var endOfLevel1 = level1Controller.FinishPosition;
        //while(level2Controller.FinishPosition.x - endOfLevel1.x <100)
            //level2.transform.Translate (Vector3.right*50);
        level1Controller.SetupPortal(level2Controller.GetPortalCoords());
        level2Controller.SetupPortal(level1Controller.GetPortalCoords());
    }
}
