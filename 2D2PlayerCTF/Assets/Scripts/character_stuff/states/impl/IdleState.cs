using UnityEngine;
using System.Collections;

public class IdleState : AbstractPlayerState {


	public IdleState(){

	}

	protected override void initializeState(){
		stateName = "idle";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		setXVelocity(controller.getRigidbody().velocity.x*3/4);
	}

}
