using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlaceAvailible : MonoBehaviour
{
    private Button _buttonAccept;
    private List<GameObject> _currentCollisions = new List<GameObject>();

    private void Start()
    {
        _buttonAccept = this.transform.Find("Canvas").transform.Find("Accept").GetComponent<Button>();
        if (_buttonAccept == null)
            Debug.LogError("BuildingPlaceAvailible: There is no Accept button");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            _currentCollisions.Add(collision.gameObject);
            if (_buttonAccept != null)
                _buttonAccept.interactable = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Building")
        {
            _currentCollisions.Remove(collision.gameObject);
            if (_currentCollisions.Count == 0 && _buttonAccept != null)
                _buttonAccept.interactable = true;
        }
    }
}

