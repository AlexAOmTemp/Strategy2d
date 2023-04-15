using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCollisionController : MonoBehaviour
{

    [SerializeField] private GameObject _distanceDetectorPrefab;
    public List<GameObject> DistanceCollisionDetectors { get; private set; } = new List<GameObject>();

    private int _idCounter = 0;
    GameObject id;
    private void Awake()
    {
        AddDistanceDetector("Vision", 6, onDistance, null);
        AddDistanceDetector("Range Attack", 5, null, onDistanceExit);
        AddDistanceDetector("Melee Attack", 2, onDistance, onDistanceExit);
        AddDistanceDetector("Aura", 4, onDistance, onDistanceExit);
        id= AddDistanceDetector("Aura", 10, onDistance, onDistanceExit);
    }
   
    public GameObject AddDistanceDetector(string name, float range, DistanceCollisionDetector.TriggerEnter triggerEnter, DistanceCollisionDetector.TriggerExit triggerExit)
    {
        var detector = Instantiate(_distanceDetectorPrefab, Vector3.zero, Quaternion.identity, this.transform);
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
            Debug.Log("Detector removed!!!!!!!!!!!!!!!!!!!");
        }
        else
            Debug.LogError("DistanceCollisionController: trying to remove unexisted collision detector");    
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
