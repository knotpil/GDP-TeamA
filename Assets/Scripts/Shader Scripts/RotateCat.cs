using UnityEngine;

public class RotateCat : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f; 
    
    void FixedUpdate()
    {
        transform.Rotate(Vector3.right * rotationSpeed * Time.fixedDeltaTime);
    }
}