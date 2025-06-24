using UnityEngine;
using UnityEngine.UI;

public class PhysicsInput : MonoBehaviour
{
    public InputField gravityField;
    public InputField massField;
    public BallController ball;

    void Start()
    {
        if (gravityField != null)
            gravityField.onEndEdit.AddListener(OnGravityChanged);
        if (massField != null)
            massField.onEndEdit.AddListener(OnMassChanged);
    }

    void OnGravityChanged(string value)
    {
        if (float.TryParse(value, out float g))
        {
            ball.gravity = Mathf.Abs(g);
            ball.UpdatePhysics();
        }
    }

    void OnMassChanged(string value)
    {
        ball.SetMass(value);
    }

    public void SetDefaults(float defaultGravity, float defaultMass)
    {
        gravityField.text = defaultGravity.ToString();
        massField.text = defaultMass.ToString();
    }
}
