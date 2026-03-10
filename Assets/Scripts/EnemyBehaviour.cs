using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyBehaviour : MonoBehaviour
{
	private Rigidbody enemyRb;
    public float enemySpeed = 15f;
	public float leftRightDuration = 2f;
	private float movementRangeX = 12f;
	public bool moveRight;

	public float stayHeightZ = 6.5f;
	public float lineStaySeconds = 10f;
	
	public float explosionStrength = 5f;
	public float explosionRadius = 5f;

	public float enemyHP = 2;
	public float enemyFireRate = 4;

	public bool isBoss;

	public GameObject bulletPrefab;

	void Start()
    {
		enemyRb = GetComponent<Rigidbody>();
        StartCoroutine(LeftRightSwitch());
		StartCoroutine(LineSwitchZ());
		StartCoroutine(ShootBullet());
	}

    void Update()
    {
		

		
		
	}

	void FixedUpdate()
	{
		//nach Unten-Bewegung
		if (enemyRb.position.z > stayHeightZ) // Enemies nach Spawn in den Bildschirm auf StandardhöheZ fliegen lassen
		{
			enemyRb.AddForce(Vector3.back * Time.deltaTime * enemySpeed, ForceMode.VelocityChange);
		}
		else if (enemyRb.position.z < stayHeightZ) // Enemies nach Überfliegen der StandardhöheZ auf diese zurücksetzen
		{
			enemyRb.position = new Vector3(enemyRb.position.x, enemyRb.position.y, stayHeightZ);
			enemyRb.linearVelocity = Vector3.zero; // Geschwindigkeit auf null setzen
		}

		//Links und Rechts Bewegung
		if (moveRight && transform.position.x < movementRangeX) //Rechts-Bewegung
		{
			enemyRb.AddForce(Vector3.right * Time.deltaTime * enemySpeed, ForceMode.VelocityChange);
		}
		else if (!moveRight && transform.position.x > -movementRangeX) //Links-Bewegung
		{
			enemyRb.AddForce(Vector3.left * Time.deltaTime * enemySpeed, ForceMode.VelocityChange);
		}

		//Begrenzung X-Achse
		if (enemyRb.position.x < -movementRangeX)
		{
			enemyRb.position = new Vector3(-movementRangeX, transform.position.y, transform.position.z); 
			enemyRb.linearVelocity = Vector3.zero; //Geschwindigkeit auf 0 setzen
		}
		else if (enemyRb.position.x > movementRangeX)
		{
			enemyRb.position = new Vector3(movementRangeX, transform.position.y, transform.position.z);
			enemyRb.linearVelocity = Vector3.zero; //Geschwindigkeit auf 0 setzen
		}
	}

    IEnumerator LeftRightSwitch() //Wechseln zwischen Links und Rechts Bewegung
    {
		moveRight = true;
        yield return new WaitForSeconds(leftRightDuration);
		enemyRb.linearVelocity = Vector3.zero;
		moveRight = false;
		yield return new WaitForSeconds(leftRightDuration);
		enemyRb.linearVelocity = Vector3.zero;
		StartCoroutine(LeftRightSwitch());
	}

	IEnumerator LineSwitchZ() // Fluginie nach unten verschieben nach x Sekunden
	{

		yield return new WaitForSeconds(lineStaySeconds);
		stayHeightZ -= 1;
		StartCoroutine(LineSwitchZ());

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy")) // Auseinanderstoßen, wenn zwei "Enemies" sich berühren
		{
			ContactPoint collisionPoint = collision.GetContact(0);
			enemyRb.linearVelocity = Vector3.zero;
			enemyRb.AddExplosionForce(explosionStrength, collisionPoint.point, explosionRadius);
		}
	}
	
	IEnumerator ShootBullet()
	{
		yield return new WaitForSeconds(enemyFireRate);
		Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		if (isBoss)
		{
			Instantiate(bulletPrefab, transform.position,Quaternion.Euler(0,30,0));
			Instantiate(bulletPrefab, transform.position,Quaternion.Euler(0,-30,0));
		}
		StartCoroutine(ShootBullet());
	}

	public void TakeDamage() //Schaden nehmen und Gegner zerstören, wenn HP=0 erreichen
	{
		enemyHP--;

		if (enemyHP <= 0 && isBoss)
		{
			GameObject.Find("Spawn Manager").GetComponent<SpawnManager>().StopAllCoroutines();
			Debug.Log("You Win!");
			Destroy(gameObject);
		}
		else if (enemyHP <= 0)
		{
			Destroy(gameObject);
		}
	}
}
