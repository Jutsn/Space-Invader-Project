using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	private Vector3 movement;
	private float horizontalInput;
	private float verticalInput;
	public float speed = 20;
	public float movementRangeX = 12f;
	public float movementRangeZ = 6.5f;
	
	public GameObject bullet;
	public float cooldownBullet = 0.9f;
	private float nextFire;

	public float playerHP = 3;

	public PowerUpType currentPowerUp;
	public float powerUpDuration = 10f;
	public float rapidFireReduction = 0.3f;

	void Start()
	{
		currentPowerUp = PowerUpType.None;
	}

	
	void Update()
	{
		MovePlayer();
		MovementRange();
		FireBullet();
	}

	void MovePlayer()
	{
		//Movement X und Z Achse
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");

		movement = new Vector3(horizontalInput, 0, verticalInput);
		transform.Translate(movement * Time.deltaTime * speed);
	}
	void MovementRange()
	{
		//Begrenzung X-Achse
		if (transform.position.x < -movementRangeX)
		{
			transform.position = new Vector3(-movementRangeX, transform.position.y, transform.position.z);
		}
		else if (transform.position.x > movementRangeX)
		{
			transform.position = new Vector3(movementRangeX, transform.position.y, transform.position.z);
		}

		//Begrenzung Z-Achse
		if (transform.position.z < -movementRangeZ)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, -movementRangeZ);
		}
		else if (transform.position.z > movementRangeZ)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, movementRangeZ);
		}
	}

	void FireBullet() //Bulletspawn durch Eingabe
	{
		// Kugel nach Cooldown abfeuern
		if (Input.GetKey(KeyCode.Space) && nextFire < Time.time)
		{
			Instantiate(bullet, transform.position + Vector3.forward, bullet.transform.rotation);
			nextFire = cooldownBullet + Time.time; //Cooldown
		}
	}

	public void TakeDamage() //HP-Abzug und Spieler-Zerstörung
	{
		playerHP--;
		if (playerHP <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Power-Up"))
		{
			currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerUpType; //Checken, welchen PowerUpType das eingesammelte Power-Up hat
			Destroy(other.gameObject);
			StartCoroutine(PowerUpPhase());
		}
	}

	IEnumerator PowerUpPhase()
	{
		if(currentPowerUp == PowerUpType.RapidFire)
		{
			cooldownBullet -= rapidFireReduction;
			yield return new WaitForSeconds(powerUpDuration);
			currentPowerUp = PowerUpType.None;
			cooldownBullet += rapidFireReduction;
		}
		
	}
}
