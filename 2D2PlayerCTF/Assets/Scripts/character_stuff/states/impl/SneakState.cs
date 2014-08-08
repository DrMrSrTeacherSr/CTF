using UnityEngine;
using System.Collections;

public class SneakState : AbstractPlayerState {

	private float sneakSpeed = 2f;
	
	
	public SneakState(){
		
	}
	
	
	protected override void initializeState(){
		stateName = "sneaking";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		if(Input.GetKey (KeyCode.A))
			setXVelocity(-sneakSpeed);
		else if(Input.GetKey (KeyCode.D))
			setXVelocity(sneakSpeed);
	}
}
