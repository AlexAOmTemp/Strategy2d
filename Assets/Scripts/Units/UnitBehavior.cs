using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    private PathPointsController _pathPointsController;
    private UnitController _unitController;
    void Start()
    {
        var currentLevel = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().CurrentLevel;
        _pathPointsController = currentLevel.GetComponent<PathPointsController>();
        _unitController = this.GetComponent<UnitController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < _pathPointsController.CurrentPathPoint)
        {
            _unitController.CurrentState = UnitController.State.RunRight;
        }

        else if (transform.position.x > _pathPointsController.CurrentPathPoint)
        {
            _unitController.CurrentState = UnitController.State.RunLeft;
        }
        else 
            _unitController.CurrentState = UnitController.State.Idle;
    }
}
