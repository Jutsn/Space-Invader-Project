using UnityEngine;

public class BulletEnemyBehaviour : MonoBehaviour
{
	public float enemyBulletSpeed = 5f;

	void Start()
    {
        
    }

    void Update()
    {
		transform.Translate(Vector3.back * Time.deltaTime * enemyBulletSpeed);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
			other.gameObject.GetComponent<PlayerController>().TakeDamage();
		}
	}
}
