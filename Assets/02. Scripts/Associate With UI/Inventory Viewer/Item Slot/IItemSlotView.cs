using UnityEngine;
using UnityEngine.EventSystems;

public interface IItemSlotView : IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    void Inject(ItemSlotPresenter presenter);

    void ClearUI();
    void UpdateUI(Sprite item_image, bool stackable, int count);
    bool IsMask(ItemType type);
}
