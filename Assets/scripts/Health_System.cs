using UnityEngine;
using System.Collections;


//Script para controlar el sistema de stats de un objeto

public class Health_System : MonoBehaviour {

	public float maxHealth = 100;
	private float currentHealth;
	public float totalArmor = 0;
	public float[] armor;
	//0 indica la armadura como una suma a la vida, 1 indica la armadura como un porcentaje sobre la vida
	public int armorType = 0;
	private float newArmor = 0;


	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
		foreach(float f in armor){
			totalArmor += f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		newArmor = 0;
		foreach(float f in armor){
			newArmor += f;
			if(totalArmor != newArmor) totalArmor = newArmor;
		}
		print (currentHealth);
	}

	public void dealDamage(float dmg){
		float newDmg = 0;
		if(armorType == 0){
			if(totalArmor <= dmg) newDmg = dmg-totalArmor;
			else newDmg = 0;
		}
		else if(armorType == 1){ //Los objetos se suman hasta alcanzar el 100%...
			if(totalArmor <= 100) newDmg = dmg-((totalArmor*dmg)/100);
			else newDmg = 0;
		}
		print("armadura: "+newDmg);
		currentHealth -= newDmg;

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
