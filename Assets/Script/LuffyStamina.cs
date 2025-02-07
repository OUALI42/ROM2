using UnityEngine;

public class LuffyStamina : MonoBehaviour
{
    
    public int maxStamina = 100;
    public float currentStamina;
    public float regen = 0.1f;


    public StaminaBar staminaBar;

    [Header("Stamina Regeneration")]
    public float staminaRegenRate = 25f; // Augmenté pour accélérer la régénération (valeur à ajuster).
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
        if(currentStamina<= maxStamina){
            currentStamina += regen;
            staminaBar.SetStamina(currentStamina);
        }
        if(currentStamina>maxStamina){
            currentStamina = maxStamina;
            staminaBar.SetStamina(currentStamina);

        }
    }

}

