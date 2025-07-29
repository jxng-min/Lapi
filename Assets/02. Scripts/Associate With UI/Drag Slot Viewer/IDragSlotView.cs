using UnityEngine;

public interface IDragSlotView
{
    void OpenUI(Sprite item_image);
    void CloseUI();
    void SetPosition(System.Numerics.Vector2 mouse_position);
}