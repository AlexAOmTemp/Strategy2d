using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCollisionDetector : MonoBehaviour
{
    private DistanceCollisionController _parentScript;
    private float _distance;
    private CircleCollider2D _collider;
    public delegate void DistanceDetectorTriggerEnter(GameObject sender, Collider2D collider);
    public delegate void DistanceDetectorTriggerExit(GameObject sender, Collider2D collider);
    private DistanceDetectorTriggerEnter _triggerEnter;
    private DistanceDetectorTriggerEnter _triggerExit;

    public float Distance 
    {
        get
        {
            return _distance;
        }
        set 
        {
            _distance = value;
            _collider.radius = _distance;
        } 
    }
    public void SetTriggerEnterAction(DistanceDetectorTriggerEnter triggerEnter)
    {
        _triggerEnter = triggerEnter;
    }
    public void SetTriggerExitAction(DistanceDetectorTriggerEnter triggerExit)
    {
        _triggerExit = triggerExit;
    }

    private void Awake()
    {
        _collider = this.GetComponent<CircleCollider2D>();
        _distance = _collider.radius;
        _parentScript = this.transform.GetComponentInParent<DistanceCollisionController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Building")|| collision.CompareTag("Unit") || collision.CompareTag("Player"))
            _triggerEnter.Invoke(this.gameObject, collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Building") || collision.CompareTag("Unit") || collision.CompareTag("Player"))
            _triggerExit.Invoke(this.gameObject, collision);
    }

}
