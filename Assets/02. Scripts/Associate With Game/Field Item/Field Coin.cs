using UnityEngine;

public class FieldCoin : FieldObject
{
    #region Variables
    [Space(30)][Header("코인 관련")]
    [Header("코인의 가치")]
    [SerializeField] private int m_amount;

    [Header("코인의 오브젝트 타입")]
    [SerializeField] private ObjectType m_type;
    #endregion Variables

    #region Properties
    public int Amount
    {
        get => m_amount;
        set => m_amount = value;
    }

    public ObjectType Type
    {
        get => m_type;
        set => m_type = value;
    }
    #endregion Properties

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            ObjectManager.Instance.ReturnObject(gameObject, Type);
        }
    }
}