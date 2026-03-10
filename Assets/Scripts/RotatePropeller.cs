using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    public Vector3 rotationAxis;
    private float rotationSpeed = 2000f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
