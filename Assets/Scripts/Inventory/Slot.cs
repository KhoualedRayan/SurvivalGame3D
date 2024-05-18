using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private ItemData itemData;

    [SerializeField]
    private Image itemVisual;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null) { 
            ToolTipSystem.instance.Show(itemData.description,itemData.name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.instance.Hide();
    }

    public void SetItemData(ItemData itemData)
    {
        this.itemData = itemData;
    }
    public void SetItemVisual(Sprite itemVisual)
    {
        this.itemVisual.sprite = itemVisual;
    }
    public void ClickOnSlot()
    {
        if(itemData != null)
        {
            Inventory.instance.OpenActionPanel(itemData, transform.position);
        }
        
    }
}
