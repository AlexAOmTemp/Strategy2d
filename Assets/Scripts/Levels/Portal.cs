using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector3 _destination;
    static private List<Collider2D> _colliders = new List<Collider2D>();
    public void Init (Vector3 destination)
    {
        _destination = destination;
        
        var size = this.GetComponentInParent<SpriteRenderer>().size;
        //Debug.Log($"{this.transform.parent.gameObject.name} {size}");
        var boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2 (size.x/2, 30);
        boxCollider.offset = new Vector2 (boxCollider.size.x/2, 0);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit") || collision.gameObject.CompareTag("Player"))
        {
            if (_colliders.Contains(collision))
                return;
            else
            {
                _colliders.Add(collision);
                collision.gameObject.transform.position = _destination;
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (_colliders.Contains(collision))
            _colliders.Remove(collision);
    }
}
