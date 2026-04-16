using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float speed = 10f;
    public float minY = -2.8f;
    public float maxY = 2.8f;

    public KeyCode upKey;
    public KeyCode downKey;

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(upKey))
            move = 1f;

        if (Input.GetKey(downKey))
            move = -1f;

        Vector3 pos = transform.position;
        pos.y += move * speed * Time.deltaTime;

        // Clamp
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}