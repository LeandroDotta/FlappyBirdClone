using UnityEngine;

public class AutoMovement : MonoBehaviour
{
    public float speed;
    public Vector2 direction;

    void Update()
    {
        Vector2 movement = direction * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
