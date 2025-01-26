using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public Transform mosquito; 
    [SerializeField] private float yOffset = 0f; 
    [SerializeField] private float lerpSpeed = 5f; 
    private Vector3 initialOffset;

    void Start()
    {
       
        initialOffset = transform.position - mosquito.position;
    }

    void Update()
    {
        
        Vector3 targetPosition = mosquito.position + initialOffset + new Vector3(0, yOffset, 0);

        
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
