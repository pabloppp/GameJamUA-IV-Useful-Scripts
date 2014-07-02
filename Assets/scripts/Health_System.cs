using UnityEngine;
using System.Collections;


//Script para controlar el sistema de stats de un objeto

public class Health_System : MonoBehaviour {

	public float maxHealth = 100;
	private float currentHealth = 100;
	public float armor = 20;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	

	
	}

	public void recieveDmg(float dmg){
		currentHealth -= dmg;
		if(currentHealth <= 0){
			currentHealth = 0;
			gameObject.SendMessage("die");
		}
	}

	public void recieveHth(float hth){
		currentHealth += hth;
		if(currentHealth > maxHealth){
			currentHealth = maxHealth;
		}
	}

}
