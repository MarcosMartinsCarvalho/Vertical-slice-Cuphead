using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform mosquito; // Assign the mosquito GameObject here
    [SerializeField] private float yOffset = 0f; // Adjustable Y offset
    private Vector3 initialOffset;

    void Start()
    {
        // Calculate the initial offset between the parent and the mosquito
        initialOffset = transform.position - mosquito.position;
    }

    void Update()
    {
        // Follow the mosquito's position with the initial offset and added yOffset
        transform.position = mosquito.position + initialOffset + new Vector3(0, yOffset, 0);
    }
}
