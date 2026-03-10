using UnityEngine;

public enum PowerUpType {None,RapidFire,Bomb}

public class PowerUp : MonoBehaviour
{ 
   public PowerUpType powerUpType;

	public float powerUpSpeed = 2f;

	private void Update()
	{
		transform.Translate(Vector3.back * Time.deltaTime * powerUpSpeed);
	}
}
    
