using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector3 direction = Vector3.right;

    private void Start()
    {
        gameObject.tag = "Bullet";
    }

    private void Update()
    {
        // Move the GameObject forward using Vector3
        this.transform.position += (direction * speed * Time.deltaTime);
        if (transform.position.x >= 15 || transform.position.x <= -15)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
}
