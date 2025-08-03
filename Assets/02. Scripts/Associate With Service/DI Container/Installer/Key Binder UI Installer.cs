using KeyService;
using UnityEngine;

public class KeyBinderUIInstaller : MonoBehaviour, IInstaller
{
    [Header("키 바인더 뷰")]
    [SerializeField] private KeyBinderView m_key_binder_view;

    public void Install()
    {
        DIContainer.Register<IKeyBinderView>(m_key_binder_view);

        var key_binder_presenter = new KeyBinderPresenter(m_key_binder_view,
                                                          ServiceLocator.Get<IKeyService>());
    }
}
