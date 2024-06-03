using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("HEALTH")]

    [SerializeField]
    private float maxHealth = 100f;
    [SerializeField]
    private Image healthBarFill;
    [SerializeField]
    private float healthDecreaseRateForHungerAndThirst;

    private float currentHealth;

    [Header("HUNGER")]

    [SerializeField]
    private float maxHunger = 100f;
    [SerializeField]
    private Image hungerBarFill;
    [SerializeField]
    private float hungerDecreaseRate; // 0.1 = 16m

    private float currentHunger;

    [Header("THIRST")]

    [SerializeField]
    private float maxThirst = 100f;
    [SerializeField]
    private Image thirstBarFill;
    [SerializeField]
    private float thirstDecreaseRate; //0.2 = 8m

    private float currentThirst;

    [Header("OTHER THINGS")]
    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHungerAndThirstBarFill();
        if(Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(2);
        }
    }

    public void TakeDamage(float damage, bool overTime = false)
    {
        if(overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }else
        {
            currentHealth -= damage;
        }
        if(currentHealth <= 0)
        {
            Debug.Log("Player Died");
        }
        animator.SetTrigger("TakeDamage");
        UpdateHealthBarFill();
    }
    private void UpdateHealthBarFill()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;

    }
    private void UpdateHungerAndThirstBarFill()
    {
        //Diminue la faim et la soif au fil du temps
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentThirst -= thirstDecreaseRate * Time.deltaTime;  

        //On empêche de passer au négatif
        currentHunger = currentHunger <0 ? 0 : currentHunger;
        currentThirst = currentThirst < 0 ? 0 : currentThirst;

        //Mise à jour des visuels
        hungerBarFill.fillAmount = currentHunger / maxHunger;
        thirstBarFill.fillAmount= currentThirst / maxThirst;

        //Si la faim ou la soif = 0 => prend des dégâts (*2 si les 2 barres sont à 0 )
        if(currentHunger <= 0 || currentThirst <= 0)
        {
            TakeDamage((currentHunger <= 0 && currentThirst <= 0 ? healthDecreaseRateForHungerAndThirst *2 : healthDecreaseRateForHungerAndThirst),true);
        }
    }
}
