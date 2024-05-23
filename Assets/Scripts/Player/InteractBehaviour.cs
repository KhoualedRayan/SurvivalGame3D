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

    /* Données non visibles dans la scène */
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
        //Bloquer les déplacements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public void DoHarvest(Harvestable harvestable)
    {
        currentTool = harvestable.GetTool();
        EnableToolGameObjectFromEnum(currentTool);
        currentHarvestable = harvestable;
        //Joue l'animation du personnage pour Miner
        playerAnimator.SetTrigger("Harvest");
        //Bloquer les déplacements du joueur lors de l'animation
        playerMoveBehaviour.SetCanMove(false);
        
    }
    public IEnumerator BreakHarvestable()
    {
        Vector3 tempOffset = spawnItemOffSet;
        //Si on doit enlever le Kinematic de l'objet a détruire
        if (currentHarvestable.IsDisableKinematicOnHarvest())
        {
            //On Récupère son rigidbody et on le passe a false
            Rigidbody rigidbody = currentHarvestable.gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            //On lui donne une légère impulsion pour que l'objet tombe
            rigidbody.AddForce(new Vector3(750,750,0),ForceMode.Impulse);
        }
        //On attend le temps nécessaire pour qu'il soit détruit
        yield return new WaitForSeconds(currentHarvestable.GetDestroyDelay());

        //Pour chaque ressource dans l'item à casser
        foreach (Ressource ressource in currentHarvestable.GetHarvestablesItems())
        {
            //On boucle avec un nombre aléatoire entre le min et le max d'items récupérables en cassant l'item
            if(Random.Range(1,101) <= ressource.dropRate)
            {
                //On les crée et les mets dans la scène à l'endroit du collectable
                GameObject instantiatiedRessource = Instantiate(ressource.itemData.prefab);
                instantiatiedRessource.transform.position = currentHarvestable.transform.position + tempOffset;
                
            }
            tempOffset.z += 0.3f;
        }
        //On détruit l'objet
        Destroy(currentHarvestable.gameObject);
    }
    public void AddItemToInventory()
    {
        //Ajoute l'objet passé à l'inventaire du joueur
        inventory.AddItem(currentItem.itemData);
        //Détruit l'objet de la scène
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
