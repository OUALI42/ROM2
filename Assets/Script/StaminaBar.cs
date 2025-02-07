using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxStamina(int Stamina)
    {
        slider.maxValue = Stamina;
        slider.value = Stamina;
    }

    public void SetStamina(float stamina)
{
    slider.value = stamina; // Assure que le slider est bien mis à jour
    // Debug.Log($"Endurance mise à jour sur la barre : {stamina}");
}

}