using UnityEngine;

public class LuffyStamina : MonoBehaviour
{
    
    public int maxStamina = 100;
    public int currentStamina;

    public StaminaBar staminaBar;

    [Header("Stamina Regeneration")]
    public int staminaRegenRate = 25; // Augmenté pour accélérer la régénération (valeur à ajuster).
    public float regenMultiplier = 2f; // Permet d'amplifier la régénération.
    public float regenDelay = 0.4f; // Temps avant de commencer la régénération.


    private float regenTimer;
    

    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    void Update()
    {
        RegenerateStamina();
    }

    public void UseStamina(int amount)
    {
        currentStamina -= 30; // point de vie enlever a la barre de stamina (ajustable)
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
        staminaBar.SetStamina(currentStamina);
        regenTimer = -regenDelay; // Réinitialise le timer de régénération
    }

    private void RegenerateStamina()
{
    // Attend un délai avant de commencer la régénération
    if (regenTimer >= 0 && regenTimer >= regenDelay)
{
   currentStamina += Mathf.FloorToInt(staminaRegenRate * regenMultiplier * Time.deltaTime);
    if (currentStamina > maxStamina)
    {
        currentStamina = maxStamina;
    }
    staminaBar.SetStamina(currentStamina);
    }
    regenTimer += Time.deltaTime;

}

}

