using UnityEngine;
using System.Collections;


//Script para controlar el sistema de stats de un objeto

public class Health_System : MonoBehaviour {

	public float maxHealth = 100;
	private float currentHealth;
	public float[] armor;
	//0 indica la armadura como una suma a la vida, 1 indica la armadura como un porcentaje sobre la vida
	public int addedArmor = 0;
	private float totalArmor = 0;


	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		foreach(float f in armor){
			totalArmor += f;
		}
		if(addedArmor == 0){
			currentHealth += totalArmor;
		}
		else if(addedArmor == 1){
			currentHealth += (totalArmor*currentHealth)/100;
		}
	}
	
	// Update is called once per frame
	void Update () {

		print (currentHealth);
	}

	public void dealDamage(float dmg){
		print ("Armadura: "+totalArmor);
		currentHealth -= dmg;

		if(currentHealth <= 0){
			currentHealth = 0;
			gameObject.SendMessage("die");
		}
	}

	public void heal(float hth){
		currentHealth += hth;
		if(currentHealth >= maxHealth){
			currentHealth = maxHealth;
		}
	}

}
