using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WorldCreator : MonoBehaviour
{
    enum Zone { 
        Castle = 0,
        InnerOne=1,
        InnerTwo=2,
        External=3
    }

    [SerializeField] private bool CastleAtLeft = true;
    [SerializeField] private int CastleZoneSize = 30;
    [SerializeField] private int FirstInnerZoneSize = 30;
    [SerializeField] private int SecondInnerZoneSize = 30;
    [SerializeField] private int ExternalZoneSize = 30;

    [SerializeField] private GameObject CastleGround;
    [SerializeField] private GameObject FirstInnerGround;
    [SerializeField] private GameObject SecondInnerGround;
    [SerializeField] private GameObject ExternalGround;

    [SerializeField] private Transform WorldStartPosition;


    private List<GameObject> groundList= new List<GameObject>();

    public void Start()
    {
        //groundList.Add(Instantiate(CastleGround, WorldStartPosition.transform.position, Quaternion.identity));
        InstatntiateGround();
    }
    public void InstatntiateGround()
    {
        groundList.Add(Instantiate(CastleGround, WorldStartPosition.transform.position, Quaternion.identity));
        SpriteRenderer renderer = groundList[0].GetComponent<SpriteRenderer>();
        Vector3 groundSize = renderer.bounds.size;
        float xDelta = groundSize.x;
        Vector3 pos = WorldStartPosition.position;
        for (int i = 1; i < CastleZoneSize; i++)
        {
            groundList.Add (Instantiate(CastleGround, pos, Quaternion.identity,this.transform) );
            pos.x += xDelta;
        }
    }

}
