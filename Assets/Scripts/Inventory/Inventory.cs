using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [Header("Inventory Panel References")]
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotParent;

    [SerializeField]
    private Transform dropPoint;

    [SerializeField]
    private Sprite emptySlotVisual;

    [Header("Action Panel References")]

    [SerializeField]
    private GameObject actionPanel;

    [SerializeField]
    private GameObject useItemButton;    
    [SerializeField]
    private GameObject equipItemButton;    
    [SerializeField]
    private GameObject dropItemButton;
    [SerializeField]
    private GameObject destroyItemButton;

    [Header("Equipments Panel References")]

    [SerializeField]
    private EquipmentLibrary equipmentLibrary;

    [SerializeField]
    private Image headSlotImage;
    [SerializeField]
    private Image chestSlotImage;
    [SerializeField]
    private Image handsSlotImage;
    [SerializeField]
    private Image legsSlotImage;
    [SerializeField]
    private Image feetSlotImage;

    /* Données non visibles sur la scène */
    const int InventorySize = 24;

    public static Inventory instance;

    private ItemData itemCurrentlySelected;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        RefreshContent();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            ToolTipSystem.instance.Hide();
            CloseActionPanel();

        }
    }
    public void AddItem(ItemData item) {
        content.Add(item); 
        RefreshContent();
    }

    public void CloseInventory()
    {
        inventoryPanel.SetActive(false);
        CloseActionPanel();
    }

    private void RefreshContent()
    {
        //On vide tous les visuels des slots
        for (int i = 0; i < inventorySlotParent.childCount; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(null);
            currentSlot.SetItemVisual(emptySlotVisual);   
        }
        //On peuple le visuel des slots selon le contenu réel de l'inventaire
        for (int i = 0; i < content.Count; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(content[i]);
            currentSlot.SetItemVisual(content[i].visual);
        }
    }
    public bool IsFull()
    {
        return InventorySize == content.Count;
    }

    public void OpenActionPanel(ItemData itemData, Vector3 slotPosition)
    {
        itemCurrentlySelected = itemData;
        switch (itemData.type)
        {
            case ItemType.Ressource:
                SetUseAndEquipButton(false, false);
                break;
            case ItemType.Equipment:
                SetUseAndEquipButton(false, true); 
                break;
            case ItemType.Consumable:
                SetUseAndEquipButton(true, false);
                break;
        }
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }
    private void SetUseAndEquipButton(bool useItem, bool equipItem)
    {
        useItemButton.SetActive(useItem);
        equipItemButton.SetActive(equipItem);
    }
    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
        RefreshContent();
    }
    public void UseItemButton()
    {
        Debug.Log("Use item : "+itemCurrentlySelected.name);    
        CloseActionPanel();
    }
    public void EquipItemButton()
    {
        //On cherche le modèle 3D de l'équipement à s'équiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem =>  elem.itemData == itemCurrentlySelected).First();
        if (equipmentLibraryItem != null)
        {
            //On désactive les visuels inutiles pour éviter le clipping
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(false);
            }
            //On active le modèle 3D de l'équipement
            equipmentLibraryItem.itemPrefab.SetActive(true);
            //On met le sprite de l'item équipé dans le slot de l'inventaire
            switch(itemCurrentlySelected.equipmentType)
            {
                case EquipmentType.Head:
                    headSlotImage.sprite = itemCurrentlySelected.visual;
                    break;

                case EquipmentType.Chest:
                    chestSlotImage.sprite = itemCurrentlySelected.visual;
                    break;

                case EquipmentType.Hands:
                    handsSlotImage.sprite = itemCurrentlySelected.visual;
                    break;

                case EquipmentType.Pants:
                    legsSlotImage.sprite = itemCurrentlySelected.visual;
                    break;

                case EquipmentType.Feet:
                    feetSlotImage.sprite = itemCurrentlySelected.visual;
                    break;
            }
            // On enlève l'item de l'invetaire et rafrachit le contenu
            content.Remove(itemCurrentlySelected);
            RefreshContent();
        }
        else
        {
            Debug.LogError("Equipement : " + itemCurrentlySelected.name + " non existant dans la librairie des équipements.");
        }
        CloseActionPanel();
    }
    public void DropItemButton()
    {
        //Déposer l'objet au dropPoint qui est dans le player
        GameObject instantiated = Instantiate(itemCurrentlySelected.prefab);
        instantiated.transform.position = dropPoint.position;
        content.Remove(itemCurrentlySelected);
        CloseActionPanel();
    }
    public void DestroyItemButton()
    {
        content.Remove(itemCurrentlySelected);
        CloseActionPanel();
    }
}
