using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSeparatorController : MonoBehaviour
{
    public float PathPointLeftY { get; private set;}
    public float PathPointRightY { get; private set; }
    public int ZoneId { get; private set; }
    public int Id { get; private set; }
    public static int CurrentZone { get; private set; } = 0;

    private GuiController _guiController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _guiController.SeparatorEntered(this.gameObject);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                CurrentZone = ZoneId + 1;
            else
                CurrentZone = ZoneId;
            _guiController.SeparatorExited(ZoneId);
        }
    }
    public void Init(int zoneId, int separatorId, float pointLeftY, float pointRightY)
    {
        _guiController = GameObject.Find("ButtonController").GetComponent<GuiController>();
        ZoneId = zoneId;
        Id = separatorId;
        PathPointLeftY = pointLeftY;
        PathPointRightY = pointRightY;
    }
    public void MoveBack()
    { 
    }
    public void MoveLeftPoint()
    { }
    public void MoveRightPoint()
    { }
    public void MoveForward()
    { 
    }

}
