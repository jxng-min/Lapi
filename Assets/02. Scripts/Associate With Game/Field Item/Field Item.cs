using UnityEngine;

public class FieldItem : FieldObject
{
    public Item Item { get; set; }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            ObjectManager.Instance.ReturnObject(gameObject, ObjectType.ITEM);
        }
    }
}