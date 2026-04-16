using UnityEngine;

public class AIPaddle : MonoBehaviour
{
    public Transform ball;
    public float speed = 8f;
    public float limitY = 2.8f;

    void Update()
    {
        if (!GameSettings.isAI) return;
        if (ball == null) return;

        Vector3 target = new Vector3(transform.position.x, ball.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float clampedY = Mathf.Clamp(transform.position.y, -limitY, limitY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }
}