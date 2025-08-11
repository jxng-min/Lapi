using UnityEngine;

public interface IShortcutSlotView
{
    void Inject(ShortcutSlotPresenter presenter);
    void UpdateUI(KeyCode code, string name);
}
