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
    //Slots d'�quipements dans l'inventaire

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
    // Boutons de d�sequipement

    [SerializeField]
    private Button headSlotDesequipButton;
    [SerializeField]
    private Button chestSlotDesequipButton;
    [SerializeField]
    private Button handsSlotDesequipButton;
    [SerializeField]
    private Button legsSlotDesequipButton;
    [SerializeField]
    private Button feetSlotDesequipButton;


    // Garde une trace des �quiments actuels
    private ItemData equipedHeadItem;
    private ItemData equipedChestItem;
    private ItemData equipedHandsItem;
    private ItemData equipedLegsItem;
    private ItemData equipedFeetItem;
    

    /* Donn�es non visibles sur la sc�ne */
    const int InventorySize = 24;

    public static Inventory instance;

    private ItemData itemCurrentlySelected;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CloseInventory();
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
        //On peuple le visuel des slots selon le contenu r�el de l'inventaire
        for (int i = 0; i < content.Count; ++i)
        {
            Slot currentSlot = inventorySlotParent.GetChild(i).GetComponent<Slot>();
            currentSlot.SetItemData(content[i]);
            currentSlot.SetItemVisual(content[i].visual);
        }
        UpdateEquipmentsDesequipButton();
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
        //On cherche le mod�le 3D de l'�quipement � s'�quiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem =>  elem.itemData == itemCurrentlySelected).First();
        if (equipmentLibraryItem != null)
        {
            //On met le sprite de l'item �quip� dans le slot de l'inventaire
            switch(itemCurrentlySelected.equipmentType)
            {
                case EquipmentType.Head:
                    DisablePreviousEquipedEquipment(equipedHeadItem);
                    headSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedHeadItem = itemCurrentlySelected;
                    break;

                case EquipmentType.Chest:
                    DisablePreviousEquipedEquipment(equipedChestItem);
                    chestSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedChestItem = itemCurrentlySelected;
                    break;

                case EquipmentType.Hands:
                    DisablePreviousEquipedEquipment(equipedHandsItem);
                    handsSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedHandsItem = itemCurrentlySelected;
                    break;

                case EquipmentType.Pants:
                    DisablePreviousEquipedEquipment(equipedLegsItem);
                    legsSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedLegsItem = itemCurrentlySelected;
                    break;

                case EquipmentType.Feet:
                    DisablePreviousEquipedEquipment(equipedFeetItem);
                    feetSlotImage.sprite = itemCurrentlySelected.visual;
                    equipedFeetItem = itemCurrentlySelected;
                    break;
            }
            //On d�sactive les visuels inutiles pour �viter le clipping
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(false);
            }
            //On active le mod�le 3D de l'�quipement
            equipmentLibraryItem.itemPrefab.SetActive(true);
            // On enl�ve l'item de l'invetaire et rafrachit le contenu
            content.Remove(itemCurrentlySelected);
            RefreshContent();
        }
        else
        {
            Debug.LogError("Equipement : " + itemCurrentlySelected.name + " non existant dans la librairie des �quipements.");
        }
        CloseActionPanel();
    }
    public void DropItemButton()
    {
        //D�poser l'objet au dropPoint qui est dans le player
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

    private void UpdateEquipmentsDesequipButton()
    {
        headSlotDesequipButton.onClick.RemoveAllListeners();
        headSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Head); });
        headSlotDesequipButton.gameObject.SetActive(equipedHeadItem);

        chestSlotDesequipButton.onClick.RemoveAllListeners();
        chestSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Chest); });
        chestSlotDesequipButton.gameObject.SetActive(equipedChestItem);

        handsSlotDesequipButton.onClick.RemoveAllListeners();
        handsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Hands); });
        handsSlotDesequipButton.gameObject.SetActive(equipedHandsItem);

        legsSlotDesequipButton.onClick.RemoveAllListeners();
        legsSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Pants); });
        legsSlotDesequipButton.gameObject.SetActive(equipedLegsItem);

        feetSlotDesequipButton.onClick.RemoveAllListeners();
        feetSlotDesequipButton.onClick.AddListener(delegate { DesequipEquipment(EquipmentType.Feet); });
        feetSlotDesequipButton.gameObject.SetActive(equipedFeetItem);

    }
    public void DesequipEquipment(EquipmentType equipmentType)
    {
        //1. Enlever le visuel de l'�quipement sur le personnage + r�-activer les parties visuelles qu'on avait d�sactiv& pour cet objet
        //2. Enlever le visuel de l'�quipement de la colonne �quipement de l'inventaire
        //3. Renvoyer l'�quipement dans l'inventaire du joueur
        //4. Refresh Content
        ItemData currentItem = null;
        if (!IsFull())
        {
            switch (equipmentType)
            {
                case EquipmentType.Head:
                    currentItem = equipedHeadItem;
                    equipedHeadItem = null;
                    headSlotImage.sprite = emptySlotVisual;
                    break;
                case EquipmentType.Chest:
                    currentItem = equipedChestItem;
                    equipedChestItem = null;
                    chestSlotImage.sprite = emptySlotVisual;
                    break;
                case EquipmentType.Hands:
                    currentItem = equipedHandsItem;
                    equipedHandsItem = null;
                    handsSlotImage.sprite = emptySlotVisual;
                    break;
                case EquipmentType.Pants:
                    currentItem = equipedLegsItem;
                    equipedLegsItem = null;
                    legsSlotImage.sprite = emptySlotVisual;
                    break;
                case EquipmentType.Feet:
                    currentItem = equipedFeetItem;
                    equipedFeetItem = null;
                    feetSlotImage.sprite = emptySlotVisual;
                    break;
            }
        }
        else
        {
            Debug.Log("L'invetaire est plein. Impossible de se d�quipper de l'item : " + equipmentType.ToString());
            return;
        }
        //On cherche le mod�le 3D de l'�quipement � se d�s�quiper
        EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == currentItem).First();
        if (equipmentLibraryItem != null)
        {
            //On active les visuels d�sactiver
            foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
            {
                elem.SetActive(true);
            }
            //On d�sactive le mod�le 3D de l'�quipement
            equipmentLibraryItem.itemPrefab.SetActive(false);
        }
        AddItem(currentItem);
        RefreshContent();
    }
    private void DisablePreviousEquipedEquipment(ItemData itemToDisable)
    {
        if(itemToDisable != null)
        {
            //On cherche le mod�le 3D de l'�quipement � se d�s�quiper
            EquipmentLibraryItem equipmentLibraryItem = equipmentLibrary.content.Where(elem => elem.itemData == itemToDisable).First();
            if (equipmentLibraryItem != null)
            {
                //On active les visuels d�sactiver
                foreach (GameObject elem in equipmentLibraryItem.elementsToDisable)
                {
                    elem.SetActive(true);
                }
                //On d�sactive le mod�le 3D de l'�quipement
                equipmentLibraryItem.itemPrefab.SetActive(false);
            }
            AddItem(itemToDisable);
        }
    }
}
