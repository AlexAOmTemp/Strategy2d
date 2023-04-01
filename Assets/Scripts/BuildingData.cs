using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class BuildingData : MonoBehaviour
{
    public string Name { get; set; }
    public void SetSprite(Sprite sprite)
    {
        this.GetComponent<SpriteRenderer>().sprite = sprite;
        this.GetComponent<BoxCollider2D>().size = sprite.bounds.size;
    }



}
