using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    private RecipeData currentRecipe;

    [SerializeField]
    private Image crafatableItemImage;

    [SerializeField]
    private GameObject elementRequiredPrefab;

    [SerializeField]
    private Transform elementsRequiredParent;

    [SerializeField]
    private Button craftButton;

    [SerializeField]
    private Sprite canBuildIcon;

    [SerializeField]
    private Sprite cantBuildIcon;

    [SerializeField]
    private Color missingColor;

    [SerializeField]
    private Color availableColor;

    public void Configure(RecipeData recipe)
    {
        currentRecipe = recipe;
        crafatableItemImage.sprite = recipe.craftableItem.visual;
        //Ajout du tooltip
        crafatableItemImage.transform.parent.GetComponent<Slot>().SetItemData(recipe.craftableItem) ;

        bool canCraft = true;
        //On fait une copie de l'inventaire pour pouvoir faire les vérifications nécessaires
        List<ItemData> invetoryCopy = new List<ItemData>(Inventory.instance.GetContent());

        for(int i = 0; i< recipe.requiredItems.Length; i++)
        {
            //On vérifie si on a tout les objets dans l'inventaire
            ItemData requiredItem = recipe.requiredItems[i];

            //On met la couleur nécessaire au fond de l'image selon si il est disponible
            GameObject requiredItemGO = Instantiate(elementRequiredPrefab, elementsRequiredParent);
            //ToolTip
            requiredItemGO.GetComponent<Slot>().SetItemData(requiredItem);

            Image requiredItemGOImage = requiredItemGO.GetComponent<Image>();

            if (invetoryCopy.Contains(requiredItem))
            {
                requiredItemGOImage.color = availableColor;
                invetoryCopy.Remove(requiredItem);
            }
            else
            {
                requiredItemGOImage.color = missingColor;
                canCraft = false;
            }

            //On instantie les images des items nécessaires au craft
            requiredItemGO.transform.GetChild(0).GetComponent<Image>().sprite = recipe.requiredItems[i].visual;
        }
        //Gestion de l'affichage du bouton
        if (!canCraft)
        {
            craftButton.image.sprite = cantBuildIcon;
            craftButton.enabled = false;
        }
        ReziseElementsRequiredParent();
    }
    private void ReziseElementsRequiredParent()
    {
        Canvas.ForceUpdateCanvases();   
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = false;
        elementsRequiredParent.GetComponent<ContentSizeFitter>().enabled = true;
    }
    public void CraftItem()
    {
        foreach (ItemData itemData in currentRecipe.requiredItems)
        {
            Inventory.instance.RemoveItem(itemData);
        }
        Inventory.instance.AddItem(currentRecipe.craftableItem);
    }
}
