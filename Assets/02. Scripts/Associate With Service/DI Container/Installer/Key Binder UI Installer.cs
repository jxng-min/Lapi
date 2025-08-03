using KeyService;
using UnityEngine;

public class KeyBinderUIInstaller : MonoBehaviour, IInstaller
{
    [Header("키 바인더 뷰")]
    [SerializeField] private KeyBinderView m_key_binder_view;

    [Header("키 바인더 슬롯의 부모 트랜스폼")]
    [SerializeField] private Transform m_key_binder_slot_root;

    public void Install()
    {
        DIContainer.Register<IKeyBinderView>(m_key_binder_view);

        var key_binder_presenter = new KeyBinderPresenter(m_key_binder_view,
                                                          ServiceLocator.Get<IKeyService>());

        var key_binder_slots = m_key_binder_slot_root.GetComponentsInChildren<KeyBinderSlotView>();
        foreach (var slot in key_binder_slots)
        {
            slot.Inject(ServiceLocator.Get<IKeyService>());
        }
    }
}
