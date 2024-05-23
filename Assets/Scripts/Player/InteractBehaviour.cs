using System.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class InteractBehaviour : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private MoveBehaviour playerMoveBehaviour;
    [Header("TOOLS VISUALS")]
    [SerializeField]
    private GameObject pickaxeVisual;
    [SerializeField]
    private GameObject axeVisual;

    /* Donn�es non visibles dans la sc�ne */
    private Item currentItem;
    private Harvestable currentHarvestable;
    private Tool currentTool;
    private Vector3 spawnItemOffSet = new Vector3(0,0.8f,0);

    public void DoPickUp(Item item)
    {
        //Si l'enventaire on ne ramasse rien
        if(inventory.IsFull() )
        {
            Debug.Log("L'inventaire est plein, impossible de ramasser : " + item.name);
            return; 
        }
        currentItem = item;
        //Joue l'animation du personnage pour ramasser l'objet
        playerAnimator.SetTrigger("PickUp");
        //Bloquer les d�placements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public void DoHarvest(Harvestable harvestable)
    {
        currentTool = harvestable.GetTool();
        EnableToolGameObjectFromEnum(currentTool);
        currentHarvestable = harvestable;
        //Joue l'animation du personnage pour Miner
        playerAnimator.SetTrigger("Harvest");
        //Bloquer les d�placements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public IEnumerator BreakHarvestable()
    {
        Vector3 tempOffset = spawnItemOffSet;
        //Si on doit enlever le Kinematic de l'objet a d�truire
        if (currentHarvestable.IsDisableKinematicOnHarvest())
        {
            //On R�cup�re son rigidbody et on le passe a false
            Rigidbody rigidbody = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            //On lui donne une l�g�re impulsion pour que l'objet tombe
            rigidbody.AddForce(new Vector3(750,750,0),ForceMode.Impulse);
        }
        //On attend le temps n�cessaire pour qu'il soit d�truit
        yield return new WaitForSeconds(currentHarvestable.GetDestroyDelay());

        //Pour chaque ressource dans l'item � casser
        foreach (Ressource ressource in currentHarvestable.GetHarvestablesItems())
        {
            //On boucle avec un nombre al�atoire entre le min et le max d'items r�cup�rables en cassant l'item
            if(Random.Range(1,101) <= ressource.dropRate)
            {
                //On les cr�e et les mets dans la sc�ne � l'endroit du collectable
                GameObject instantiatiedRessource = Instantiate(ressource.itemData.prefab);
                instantiatiedRessource.transform.position = currentHarvestable.transform.position + tempOffset;
                
            }
            tempOffset.z += 0.3f;
        }
        //On d�truit l'objet
        Destroy(currentHarvestable.gameObject);
    }
    public void AddItemToInventory()
    {
        //Ajoute l'objet pass� � l'inventaire du joueur
        inventory.AddItem(currentItem.itemData);
        //D�truit l'objet de la sc�ne
        Destroy(currentItem.gameObject);
    }
    public void ReEnablePlayerMovements()
    {
        playerMoveBehaviour.SetCanMove(true);
        EnableToolGameObjectFromEnum(currentTool,false);
    }
    private void EnableToolGameObjectFromEnum(Tool toolType,bool enabled = true)
    {
        switch (toolType)
        {
            case Tool.Pickaxe:
                pickaxeVisual.SetActive(enabled); 
                break;
            case Tool.Axe:
                axeVisual.SetActive(enabled);
                break;
        }
    }
}
