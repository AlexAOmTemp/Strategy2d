using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlaceAvailible : MonoBehaviour
{
    [SerializeField]private Button _buttonAccept;
    private List<GameObject> _currentCollisions = new List<GameObject>();

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

