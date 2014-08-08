using UnityEngine;
using System.Collections;

public class WalkingState : AbstractPlayerState {

	private float walkSpeed = 4f;


	public WalkingState(){
		
	}


	protected override void initializeState(){
		stateName = "walking";
	}

	public override void updateState(PlayerController controller){
		this.controller = controller;
		if(Input.GetKey (KeyCode.A))
			setXVelocity(-walkSpeed);
		else
			setXVelocity(walkSpeed);
	}

}
