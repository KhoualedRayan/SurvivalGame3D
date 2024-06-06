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

    public float currentHealth;

    [Header("HUNGER")]

    [SerializeField]
    private float maxHunger = 100f;
    [SerializeField]
    private Image hungerBarFill;
    [SerializeField]
    private float hungerDecreaseRate; // 0.1 = 16m

    public float currentHunger;

    [Header("THIRST")]

    [SerializeField]
    private float maxThirst = 100f;
    [SerializeField]
    private Image thirstBarFill;
    [SerializeField]
    private float thirstDecreaseRate; //0.2 = 8m

    public float currentThirst;

    [Header("OTHER THINGS")]

    [SerializeField]
    private Animator healthBarAnimator;

    [SerializeField]
    private Animator playerAnimator;

    [SerializeField]
    private float currentArmorPoints;

    [SerializeField]
    private MoveBehaviour playerMovementScript;

    private bool isDead = false;

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
            TakeDamage(50f);
        }
    }

    public void TakeDamage(float damage, bool overTime = false)
    {
        if(overTime)
        {
            currentHealth -= damage * Time.deltaTime;
        }else
        {
            currentHealth -= damage * (1 - (currentArmorPoints / 100) );
        }
        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
        healthBarAnimator.SetTrigger("TakeDamage");
        UpdateHealthBarFill();
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player Died");
        playerMovementScript.SetCanMove(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<AimBehaviourBasic>().enabled = false;
        playerAnimator.SetTrigger("Die");
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

        //On emp�che de passer au n�gatif
        currentHunger = currentHunger <0 ? 0 : currentHunger;
        currentThirst = currentThirst < 0 ? 0 : currentThirst;

        //Mise � jour des visuels
        hungerBarFill.fillAmount = currentHunger / maxHunger;
        thirstBarFill.fillAmount= currentThirst / maxThirst;

        //Si la faim ou la soif = 0 => prend des d�g�ts (*2 si les 2 barres sont � 0 )
        if(currentHunger <= 0 || currentThirst <= 0)
        {
            TakeDamage((currentHunger <= 0 && currentThirst <= 0 ? healthDecreaseRateForHungerAndThirst *2 : healthDecreaseRateForHungerAndThirst),true);
        }
    }
    public void ConsumeItem(float health, float hunger, float thirst)
    {
        currentHealth = Mathf.Min(maxHealth, health + currentHealth);
        currentHunger = Mathf.Min(maxHunger, hunger + currentHunger);
        currentThirst = Mathf.Min(maxThirst, thirst + currentThirst);
        UpdateHealthBarFill();
    }

    /* GETTERS */
    public float GetCurrentArmorPoints()
    {
        return currentArmorPoints;
    }
    public bool IsDead()
    {
        return isDead;
    }
    /* SETTERS */
    public void AddArmorPoints(float armor)
    {
        this.currentArmorPoints += armor;
    }
    public void RemoveArmorPoints(float armor)
    {
        this.currentArmorPoints -= armor;
    }
    public void RemoveAndAddArmorPoints(float addedArmorPoints,float removedArmorPoints)
    {
        this.currentArmorPoints += addedArmorPoints;
        this.currentArmorPoints -= removedArmorPoints;
    }
}
