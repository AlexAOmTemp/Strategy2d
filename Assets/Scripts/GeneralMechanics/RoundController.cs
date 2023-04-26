using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField] private LevelSpawner _levelSpawner;
    private List<GameObject> _levels;
    static public List<float> PathPoints { get; private set; } = new List<float>();
    static public int PlayerLavelId = 0;
    static public string PlayerRace = "Human";
    void Start()
    {
        var level1 = _levelSpawner.CreateLevel(0, false, true);
        var level2 = _levelSpawner.CreateLevel(1, true, false);

        var level1Controller = level1.GetComponent<LevelController>();
        var level2Controller = level2.GetComponent<LevelController>();
        var endOfLevel1 = level1Controller.GetFinishPosition();
        var endOfLevel2 = level2Controller.GetFinishPosition();

        if (endOfLevel1.x > endOfLevel2.x)
        {
            level2.transform.Translate(Vector3.right * 200);
            collectPoints(level1Controller.GetPathPoints(), level2Controller.GetPathPoints());
            IndexSeparators(level1Controller.GetZoneSeparators(),level2Controller.GetZoneSeparators() );
        }
        else
        {
            level1.transform.Translate(Vector3.right * 200);
            collectPoints(level2Controller.GetPathPoints(), level1Controller.GetPathPoints());
            IndexSeparators(level2Controller.GetZoneSeparators(),level1Controller.GetZoneSeparators() );
        }
        level1Controller.SetupPortal(level2Controller.GetPortalCoords());
        level2Controller.SetupPortal(level1Controller.GetPortalCoords());
    }
    private void collectPoints(List<GameObject> directLevelPoints, List<GameObject> reverseLevelPoints)
    {
        directLevelPoints.RemoveAt(directLevelPoints.Count-1);
        reverseLevelPoints.RemoveAt(reverseLevelPoints.Count-1);
        PathPointsController.PathPoints.AddRange(directLevelPoints);
        reverseLevelPoints.Reverse();
        PathPointsController.PathPoints.AddRange(reverseLevelPoints);
    }
    private void IndexSeparators(List<GameObject> directLevelSeparators, List<GameObject> reverseLevelSeparators)
    {
        int id =0;
        foreach (var separator in directLevelSeparators)
            separator.GetComponent<SeparatorController>().Id = id++;
        id--; //two portails count as one
        reverseLevelSeparators.Reverse();
        foreach (var separator in reverseLevelSeparators)
            separator.GetComponent<SeparatorController>().Id = id++;
        
    }
}
