using UnityEngine;
using UnityEngine.UI;

public class EnergyGraph : MonoBehaviour
{
    public Image peBar;
    public Image keBar;
    public float maxEnergy = 100f;

    public void UpdateEnergy(float potential, float kinetic)
    {
        float total = potential + kinetic;
        peBar.fillAmount = Mathf.Clamp01(potential / maxEnergy);
        keBar.fillAmount = Mathf.Clamp01(kinetic / maxEnergy);
    }
}