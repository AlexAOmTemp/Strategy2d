using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCollisionController : MonoBehaviour
{
    [SerializeField] private GameObject _distanceDetectorPrefab;
    public List<GameObject> DistanceCollisionDetectors { get; private set; } = new List<GameObject>();
    private void Awake()
    {
        AddDistanceDetector("Vision", 6);
        AddDistanceDetector("Range Attack", 5);
        AddDistanceDetector("Melee Attack", 2);
        AddDistanceDetector("Aura", 4);
        AddDistanceDetector("Aura", 4);
    }
    public void AddDistanceDetector(string name, float range)
    {
        if (DistanceCollisionDetectors.Find(i => i.name == name) != null)
            Debug.LogError("DistanceCollisionController: trying to add a new collision detector with existing name");
        else
        {
            var detector = Instantiate(_distanceDetectorPrefab, Vector3.zero, Quaternion.identity, this.transform);
            var detectorScript = detector.GetComponent<DistanceCollisionDetector>();
            detectorScript.Distance = range;
            detector.name = name;
            detectorScript.SetTriggerEnterAction(onDistance);
            detectorScript.SetTriggerExitAction(onDistanceExit);
            DistanceCollisionDetectors.Add(detector);
        }
    }
    public void RemoveDistanceDetector(string name)
    {
        var detector = DistanceCollisionDetectors.Find(i => i.name == name);
        if (detector == null)
            Debug.LogError("DistanceCollisionController: trying to remove unexisted collision detector");
        else
            DistanceCollisionDetectors.Remove(detector);
    }

    public void onDistance(GameObject sender, Collider2D collider)
    {
        Debug.Log($"Game Object {collider.name} on the {sender.name} distance");
    }
    public void onDistanceExit(GameObject sender, Collider2D collider)
    {
        Debug.Log($"Game Object {collider.name} has left the {sender.name} distance");
    }
}
