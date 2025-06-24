using UnityEngine;

public class ResetSimulation : MonoBehaviour
{
    public BallController ball;

    public void Reset()
    {
        if (ball != null)
        {
            ball.transform.position = new Vector3(-2, 2, 0);  // Reset position
            ball.rigid.linearVelocity = Vector3.zero;               // Reset velocity
            ball.rigid.angularVelocity = Vector3.zero;        // Reset any spin

            // Reset energy bars visually
            if (ball.peColumn != null)
                ball.peColumn.sizeDelta = new Vector2(ball.peColumn.sizeDelta.x, 0);

            if (ball.keColumn != null)
                ball.keColumn.sizeDelta = new Vector2(ball.keColumn.sizeDelta.x, 0);

            // Reset velocity text
            if (ball.velocityText != null)
                ball.velocityText.text = "Velocity: 0 m/s";
        }
    }
}
