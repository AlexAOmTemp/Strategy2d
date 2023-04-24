using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSeparatorController : MonoBehaviour
{
    public int ZoneId { get; private set; }
    public int Id { get; private set; }
    public static int CurrentZone { get; private set; } = 0;

    private GuiController _guiController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("PlayerInBuilding") )
            _guiController.SeparatorEntered(this.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerInBuilding") )
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                CurrentZone = ZoneId;         
            else
                CurrentZone = ZoneId-1;
            _guiController.SeparatorExited(CurrentZone);
        }
    }
    public void Init(int zoneId, int separatorId, Sprite image)
    {
        _guiController = GameObject.Find("ButtonController").GetComponent<GuiController>();
        ZoneId = zoneId;
        Id = separatorId;
        var renderer = this.GetComponent<SpriteRenderer>();
        renderer.sprite = image;
        var boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2 (renderer.bounds.size.x,  30f);
    }
}
