using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 8f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    void Launch()
    {
        float x = Random.value > 0.5f ? 1f : -1f;
        float y = Random.Range(-0.5f, 0.5f);

        Vector2 direction = new Vector2(x, y).normalized;
        rb.linearVelocity = direction * speed;
    }

    public void StopBall()
    {
        rb.linearVelocity = Vector2.zero;
    }

    public void ResetBall()
    {
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector2.zero;
        Launch();
    }
    void FixedUpdate()
    {
        if (Mathf.Abs(rb.linearVelocity.y) < 0.5f)
        {
            float newY = rb.linearVelocity.y >= 0 ? 0.5f : -0.5f;
            Vector2 fixedVelocity = new Vector2(rb.linearVelocity.x, newY).normalized * speed;
            rb.linearVelocity = fixedVelocity;
        }
    }
}