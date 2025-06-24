using UnityEngine;
using UnityEngine.UI;

public class EnergyBarChart : MonoBehaviour
{
    public Image PEBar;
    public Image KEBar;

    private float maxEnergy = 100f;

    public void UpdateEnergy(float pe, float ke)
    {
        float total = Mathf.Max(pe + ke, 1f);
        maxEnergy = Mathf.Max(maxEnergy, total);

        if (PEBar != null)
            PEBar.fillAmount = Mathf.Clamp01(pe / maxEnergy);

        if (KEBar != null)
            KEBar.fillAmount = Mathf.Clamp01(ke / maxEnergy);
    }

    public void ResetBars()
    {
        if (PEBar != null) PEBar.fillAmount = 0f;
        if (KEBar != null) KEBar.fillAmount = 0f;
        maxEnergy = 100f;
    }
}