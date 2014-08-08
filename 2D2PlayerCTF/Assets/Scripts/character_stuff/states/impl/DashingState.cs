using UnityEngine;
using System.Collections;

public class DashingState : AbstractPlayerState{


	private float dashSpeed = 8f;


	public DashingState(){
		
	}

	protected override void initializeState(){
		stateName = "dashing";
	}

	public override void updateState(PlayerController controller){
		this.controller = controller;
		if(Input.GetKey (KeyCode.A))
			setXVelocity(-dashSpeed);
		else
			setXVelocity(dashSpeed);
	}

}
