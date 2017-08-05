using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	List<Hero> heroes;

	// Use this for initialization
	void Start () {

		heroes = new List<Hero>();

		Hero heroCyril = new Hero ("Cyril");
		heroCyril.hp = 540;
		heroCyril.hpMax = 720;

		heroes.Add(heroCyril);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
