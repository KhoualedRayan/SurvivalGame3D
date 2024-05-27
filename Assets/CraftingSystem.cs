using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField]
    private List<RecipeData> availableRecipes = new List<RecipeData>();
    [SerializeField]
    private GameObject recipeUiPrefab;
    [SerializeField]
    private Transform recipesParent;
    [SerializeField]
    private KeyCode openCraftPanelInput;
    [SerializeField]
    private GameObject craftingPanel;

    void Start()
    {
        UpdateDisplayRecipes();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(openCraftPanelInput)) 
        {
            craftingPanel.SetActive(!craftingPanel.activeSelf);
            UpdateDisplayRecipes();
        }
    }
    public void UpdateDisplayRecipes()
    {
        //On supprime tt les crafts
        foreach(Transform child in recipesParent)
        {
            Destroy(child.gameObject);
        }
        //On ajoute tt les crafts
        for(int i =0; i<availableRecipes.Count ; ++i)
        {
            GameObject recipe = Instantiate(recipeUiPrefab,recipesParent);
            recipe.GetComponent<Recipe>().Configure(availableRecipes[i]);   
        }
    }
}
