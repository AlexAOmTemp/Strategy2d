using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMain : MonoBehaviour
{
    private UnitController _unitController;
    private UnitBehavior _unitBehavior;
    public UnitData UnitData { get; private set; }
    private SpriteRenderer spriteRenderer;
    private DistanceCollisionController distanceCollisionController;
    public void Init(UnitData unitData)
    {
        UnitData = unitData;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        distanceCollisionController = this.GetComponent<DistanceCollisionController>();
        this.name = UnitData.Name;
        spriteRenderer.sprite = unitData.SpriteAlive;
        this.GetComponent<BoxCollider2D>().size = spriteRenderer.size;
        this.GetComponent<UnitController>().Init(unitData);
        _unitBehavior =  this.GetComponent<UnitBehavior>();
        _unitBehavior.UnitIsInPOint+= onUnitReachPoint;
    }
    void onUnitReachPoint()
    {
        Debug.Log("Yes he does!");
    }

}
