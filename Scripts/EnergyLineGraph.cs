using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnergyLineGraph : MonoBehaviour
{
    public RectTransform graphContainer;
    public GameObject pointPrefab;
    public Color kineticColor = Color.red;
    public Color potentialColor = Color.blue;

    private float timeStep = 0.1f;
    private float timer = 0f;
    private float maxHeight = 100f;
    private List<GameObject> kePoints = new List<GameObject>();
    private List<GameObject> pePoints = new List<GameObject>();
    private float graphWidth;

    void Start()
    {
        graphWidth = graphContainer.sizeDelta.x;
    }

    public void UpdateEnergy(float pe, float ke)
    {
        timer += Time.deltaTime;
        if (timer >= timeStep)
        {
            timer = 0f;
            AddPoint(pe, potentialColor, pePoints);
            AddPoint(ke, kineticColor, kePoints);
        }
    }

    void AddPoint(float value, Color color, List<GameObject> pointsList)
    {
        float x = pointsList.Count * 5f;
        if (x > graphWidth) return;

        float y = Mathf.Clamp(value / maxHeight * graphContainer.sizeDelta.y, 0, graphContainer.sizeDelta.y);
        GameObject point = Instantiate(pointPrefab, graphContainer);
        point.GetComponent<Image>().color = color;
        point.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        pointsList.Add(point);
    }

    public void ResetGraph()
    {
        foreach (var p in kePoints) Destroy(p);
        foreach (var p in pePoints) Destroy(p);
        kePoints.Clear();
        pePoints.Clear();
    }
}
