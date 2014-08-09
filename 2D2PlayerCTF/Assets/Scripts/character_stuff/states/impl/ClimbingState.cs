using UnityEngine;
using System.Collections;

public class ClimbingState : AbstractPlayerState {

	private float climbingSpeed = 4f;
	
	
	public ClimbingState(){
		
	}
	
	
	protected override void initializeState(){
		stateName = "climbing";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;

		if(Input.GetKey(KeyCode.A))
			setXVelocity(climbingSpeed * Input.GetAxis("Horizontal"));
		else if(Input.GetKey (KeyCode.D))
			setXVelocity(climbingSpeed* Input.GetAxis("Horizontal"));

		if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
			setXVelocity(0);
		}


		if(Input.GetKey (KeyCode.S))
			setYVelocity(-climbingSpeed);
		else if(Input.GetKey (KeyCode.W))
			setYVelocity(climbingSpeed);
	}
}
