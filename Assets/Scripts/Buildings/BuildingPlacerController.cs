using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacerController : MonoBehaviour
{
    private GameObject _player=null;

    public void setPlayer(GameObject player)
    {
        _player = player;
    }
    void Update()
    {
        if (_player!=null)
            transform.position = new Vector3(_player.transform.position.x, transform.position.y, 0);
    }
}
