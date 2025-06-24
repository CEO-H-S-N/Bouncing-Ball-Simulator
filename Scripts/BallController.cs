using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public Rigidbody rigid;
    public float gravity = 9.8f;
    public float mass = 1.0f;

    public RectTransform peColumn;
    public RectTransform keColumn;
    public Text velocityText;

    public Transform ground;

    private float maxEnergy = 100f;

    private float GetPotentialEnergy()
    {
        float height = transform.position.y - ground.position.y;
        return Mathf.Max(0, mass * gravity * height);
    }

    private float GetKineticEnergy()
    {
        return 0.5f * mass * rigid.linearVelocity.sqrMagnitude;
    }

    void Start()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();

        ApplyPhysicsSettings();
    }

    void Update()
    {
        if (rigid == null || ground == null)
            return;

        float pe = GetPotentialEnergy();
        float ke = GetKineticEnergy();
        float total = pe + ke;

        float peHeight = Mathf.Clamp((pe / maxEnergy) * 200f, 0, 200f);
        float keHeight = Mathf.Clamp((ke / maxEnergy) * 200f, 0, 200f);

        if (peColumn != null)
            peColumn.sizeDelta = new Vector2(peColumn.sizeDelta.x, peHeight);

        if (keColumn != null)
            keColumn.sizeDelta = new Vector2(keColumn.sizeDelta.x, keHeight);

        if (velocityText != null)
            velocityText.text = $"Velocity: {rigid.linearVelocity.magnitude:F2} m/s";
    }
public void ResetBall()
{
    if (rigid != null)
    {
        rigid.linearVelocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    transform.position = new Vector3(-2, 2, 0); // Adjust as needed
}

    public void SetMass(string massStr)
    {
        if (float.TryParse(massStr, out float m))
        {
            mass = m;
            if (rigid != null)
                rigid.mass = m;
        }
    }
public void ApplyPhysicsSettings()
{
    if (rigid != null)
    {
        rigid.mass = mass;
    }

    Physics.gravity = new Vector3(0, -Mathf.Abs(gravity), 0);
}

    public void SetGravity(string gravityStr)
    {
        if (float.TryParse(gravityStr, out float g))
        {
            gravity = Mathf.Abs(g);
            Physics.gravity = new Vector3(0, -gravity, 0);
        }
    }

    public void UpdatePhysics()
    {
        if (rigid != null)
        {
            rigid.mass = mass;
            Physics.gravity = new Vector3(0, -Mathf.Abs(gravity), 0);
        }
    }
}
