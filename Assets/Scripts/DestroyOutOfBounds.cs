using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float bulletrangeZTop = 9.1f;
    private float bulletrangeZBot = -12f;

    void Start()
    {
        
    }

    void Update()
    {
        //Zerstöre am oberen Bildschirmrand
        if (transform.position.z > bulletrangeZTop)
        {
            Destroy(gameObject);
        }
        //Zerstöre am unteren Bildschirmrand
        if (transform.position.z < bulletrangeZBot)
        {
            Destroy(gameObject);
        }
    }
}
