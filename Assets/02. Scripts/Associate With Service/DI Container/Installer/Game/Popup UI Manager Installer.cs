using System.Collections.Generic;
using UnityEngine;

public class PopupUIManagerInstaller : MonoBehaviour, IInstaller
{
    [Header("팝업UI 매니저")]
    [SerializeField] private PopupUIManager m_popup_manager;
    public void Install()
    {
        var popup_data_list = new List<PopupData>{
            new("Inventory", DIContainer.Resolve<InventoryPresenter>()),
            new("Equipment", DIContainer.Resolve<EquipmentPresenter>()),
            new("Skill", DIContainer.Resolve<SkillPresenter>()),
            new("Binder", DIContainer.Resolve<KeyBinderPresenter>()),
            new("Shortcut", DIContainer.Resolve<ShortcutPresenter>()),
            new("Pause", DIContainer.Resolve<PausePresenter>()),
        };

        m_popup_manager.Inject(popup_data_list);
    }
}
