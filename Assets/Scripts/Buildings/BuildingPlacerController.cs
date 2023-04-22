using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacerController : MonoBehaviour
{
    private GameObject _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(_player.transform.position.x, transform.position.y, 0);
    }
}
