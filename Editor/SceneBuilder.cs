// SceneBuilder.cs
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SceneBuilderEditor : EditorWindow
{
    [MenuItem("Tools/Build Bouncing Ball Scene")]
    public static void BuildSceneLayout()
    {
        // Clear Scene (except Main Camera)
        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj != null && obj.name != "Main Camera")
                DestroyImmediate(obj);
        }

        // Configure Main Camera
        Camera.main.backgroundColor = Color.black;
        Camera.main.transform.position = new Vector3(0, 2, -10);
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5;

        // Ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0, -2.5f, 0);
        ground.transform.localScale = new Vector3(10, 1, 1);
        ground.GetComponent<Renderer>().sharedMaterial.color = Color.gray;

        // Ball
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "Ball";
        ball.transform.position = new Vector3(-2, 2, 0);
        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.mass = 1f;

        BallController ballController = ball.AddComponent<BallController>();
        ballController.rigid = rb;
        ballController.mass = rb.mass;
        ballController.gravity = Mathf.Abs(Physics.gravity.y);
        ballController.ground = ground.transform;

        // Canvas Setup
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // EventSystem
        if (Object.FindObjectOfType<EventSystem>() == null)
        {
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        }

        // Energy Bars
        GameObject peBar = CreateBar("PE", new Vector2(0.8f, 0.4f), Color.red);
        GameObject keBar = CreateBar("KE", new Vector2(0.9f, 0.4f), Color.blue);
        peBar.transform.SetParent(canvas.transform, false);
        keBar.transform.SetParent(canvas.transform, false);
        ballController.peColumn = peBar.GetComponent<RectTransform>();
        ballController.keColumn = keBar.GetComponent<RectTransform>();

        // Velocity Text
        GameObject velocityGO = CreateText("VelocityText", "Velocity: 0 m/s", new Vector2(0.5f, 0.95f));
        velocityGO.transform.SetParent(canvas.transform, false);
        ballController.velocityText = velocityGO.GetComponent<Text>();

        // Reset Button
        GameObject resetButtonGO = CreateButton("Reset", new Vector2(0.1f, 0.9f));
        resetButtonGO.transform.SetParent(canvas.transform, false);
        Button resetButton = resetButtonGO.GetComponent<Button>();
        ResetSimulation resetScript = resetButtonGO.AddComponent<ResetSimulation>();
        resetScript.ball = ballController;
        resetButton.onClick.AddListener(() => resetScript.Reset());

        // Input Fields
        GameObject gravityInputGO = CreateInput("GravityInput", "Gravity", new Vector2(0.1f, 0.7f));
        GameObject massInputGO = CreateInput("MassInput", "Mass", new Vector2(0.1f, 0.6f));
        gravityInputGO.transform.SetParent(canvas.transform, false);
        massInputGO.transform.SetParent(canvas.transform, false);

        // Input Script
        GameObject inputManager = new GameObject("PhysicsInput");
        PhysicsInput inputScript = inputManager.AddComponent<PhysicsInput>();
        inputScript.gravityField = gravityInputGO.GetComponent<InputField>();
        inputScript.massField = massInputGO.GetComponent<InputField>();
        inputScript.ball = ballController;
    }

    static GameObject CreateBar(string name, Vector2 anchor, Color color)
    {
        GameObject bar = new GameObject(name);
        RectTransform rt = bar.AddComponent<RectTransform>();
        Image img = bar.AddComponent<Image>();
        img.color = color;
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.sizeDelta = new Vector2(20, 100);
        return bar;
    }

    static GameObject CreateText(string name, string content, Vector2 anchor)
    {
        GameObject go = new GameObject(name);
        RectTransform rt = go.AddComponent<RectTransform>();
        Text txt = go.AddComponent<Text>();
        txt.text = content;
        txt.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        txt.fontSize = 16;
        txt.color = Color.white;
        txt.alignment = TextAnchor.MiddleCenter;
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.sizeDelta = new Vector2(200, 30);
        return go;
    }

    static GameObject CreateButton(string label, Vector2 anchor)
    {
        GameObject go = new GameObject(label);
        RectTransform rt = go.AddComponent<RectTransform>();
        Image img = go.AddComponent<Image>();
        img.color = Color.gray;
        Button btn = go.AddComponent<Button>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.sizeDelta = new Vector2(100, 30);

        GameObject txtGO = CreateText("Text", label, new Vector2(0.5f, 0.5f));
        txtGO.transform.SetParent(go.transform, false);

        return go;
    }

    static GameObject CreateInput(string name, string placeholderText, Vector2 anchor)
    {
        GameObject go = new GameObject(name);
        RectTransform rt = go.AddComponent<RectTransform>();
        Image img = go.AddComponent<Image>();
        img.color = Color.white;
        InputField input = go.AddComponent<InputField>();
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.sizeDelta = new Vector2(100, 30);

        Text text = CreateText("Text", "", new Vector2(0.5f, 0.5f)).GetComponent<Text>();
        text.transform.SetParent(go.transform, false);
        input.textComponent = text;

        Text placeholder = CreateText("Placeholder", placeholderText, new Vector2(0.5f, 0.5f)).GetComponent<Text>();
        placeholder.color = Color.gray;
        placeholder.transform.SetParent(go.transform, false);
        input.placeholder = placeholder;

        return go;
    }
}
