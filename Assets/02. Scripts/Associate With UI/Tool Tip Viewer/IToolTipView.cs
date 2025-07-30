using UnityEngine;

public interface IToolTipView
{
    void Inject(ToolTipPresenter presenter);

    void OpenUI();
    void UpdateUI(Sprite image, string name, string type, string description);
    void CloseUI();
}