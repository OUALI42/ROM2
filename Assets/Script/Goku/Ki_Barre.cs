using UnityEngine;
using UnityEngine.UI;
public class Ki_Barre : MonoBehaviour
{
    public Slider slider;
    [Header("Ki")]
    public int maxKi = 50; // Le max de Ki que Goku peut avoir
    public int currentKi = 0; // Le Ki actuel de Goku
    public Ki_Barre kiBar; // Référence à la barre de Ki
    private GokuAnimAttack goku;

    void Start()
    {
        goku = FindObjectOfType<GokuAnimAttack>(); // Récupère Goku
        SetMaxKi(maxKi);
        Ki_gestion();
    }
    
    public void SetMaxKi(int ki)
    {
        slider.maxValue = ki;
        slider.value = ki;
    }
    public void SetKi(int ki)
    {
        slider.value = ki;
    }
    
    public void Ki_gestion()
    {
        currentKi = goku.Ki; // Met à jour la valeur actuelle du Ki
        SetKi(currentKi);
    }
}
