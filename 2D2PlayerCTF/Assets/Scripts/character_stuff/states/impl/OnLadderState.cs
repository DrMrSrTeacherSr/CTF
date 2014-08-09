using UnityEngine;
using System.Collections;

public class OnLadderState : AbstractPlayerState {

	private float climbingSpeed = 4f;

	public OnLadderState(){

	}
	
	
	protected override void initializeState(){
		stateName = "onLadder";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		setYVelocity(0f);

		if(Input.GetKey(KeyCode.A))
			setXVelocity(climbingSpeed * Input.GetAxis("Horizontal"));
		else if(Input.GetKey (KeyCode.D))
			setXVelocity(climbingSpeed* Input.GetAxis("Horizontal"));
		
		if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
			setXVelocity(0);
		}


	}
}
