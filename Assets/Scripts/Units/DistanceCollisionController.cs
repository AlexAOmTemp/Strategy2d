using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCollisionController : MonoBehaviour
{
    [SerializeField] private GameObject _distanceDetectorPrefab;
    public List<GameObject> DistanceCollisionDetectors { get; private set; } = new List<GameObject>();

    //private int _idCounter = 0;
    //GameObject id;
    public delegate void UnitAtDistance(GameObject obj);
    public event UnitAtDistance MeleeDistance;
    public event UnitAtDistance RangeDistance;
    public event UnitAtDistance VisionDistance;
    public event UnitAtDistance MeleeDistanceLeft;
    public event UnitAtDistance RangeDistanceLeft;
    public event UnitAtDistance VisionDistanceLeft;

    public void Init(float? visionDistance, float? rangeAttackDistance, float? meleeAttackDistance)
    {
        if (visionDistance != null)
            AddDistanceDetector("Vision", (float)visionDistance, onDistance, onDistanceExit);
        if (rangeAttackDistance != null)
            AddDistanceDetector("Range Attack", (float)rangeAttackDistance, onDistance, onDistanceExit);
        if (meleeAttackDistance != null)
            AddDistanceDetector("Melee Attack", (float)meleeAttackDistance, onDistance, onDistanceExit);
    }

    public GameObject AddDistanceDetector(string name, float range, DistanceCollisionDetector.TriggerEnter triggerEnter, DistanceCollisionDetector.TriggerExit triggerExit)
    {
        var detector = Instantiate(_distanceDetectorPrefab, this.transform.position, Quaternion.identity, this.transform);
        var detectorScript = detector.GetComponent<DistanceCollisionDetector>();
        detectorScript.Distance = range;
        detector.name = name;
        detectorScript.SetTriggerEnterAction(triggerEnter);
        detectorScript.SetTriggerExitAction(triggerExit);
        DistanceCollisionDetectors.Add(detector);
        return detector;
    }
    public void RemoveDistanceDetector(GameObject detector)
    {
        if (DistanceCollisionDetectors.Contains(detector))
        {
            DistanceCollisionDetectors.Remove(detector);
            Destroy(detector);
        }
        else
            Debug.LogError("DistanceCollisionController: trying to remove unexisted collision detector");
    }

    public void onDistance(GameObject sender, Collider2D collider)
    {
        if (sender.name == "Vision")
            VisionDistance?.Invoke(collider.gameObject);
        if (sender.name == "Range Attack")
            RangeDistance?.Invoke(collider.gameObject);
        if (sender.name == "Melee Attack")
            MeleeDistance?.Invoke(collider.gameObject);
        Debug.Log($"DistanceCollisionController: Game Object {collider.name} on the {sender.name} distance of unit {gameObject.name}");
    }
    public void onDistanceExit(GameObject sender, Collider2D collider)
    {
        if (sender.name == "Vision")
            VisionDistanceLeft?.Invoke(collider.gameObject);
        if (sender.name == "Range Attack")
            RangeDistanceLeft?.Invoke(collider.gameObject);
        if (sender.name == "Melee Attack")
            MeleeDistanceLeft?.Invoke(collider.gameObject);
        Debug.Log($"DistanceCollisionController: Game Object {collider.name} has left the {sender.name} distance of unit {gameObject.name}");
    }
}
