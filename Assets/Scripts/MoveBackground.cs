using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private Vector3 startPos;
    public float backgroundSpeed;
    
	void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.back * backgroundSpeed * Time.deltaTime);
        if(transform.position.z <= -37.1)
        {
			transform.position = startPos;
		}
        
    }
}
