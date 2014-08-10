using UnityEngine;
using System.Collections;

public class DoubleJumpingState : AbstractPlayerState {

	private float jumpSpeed;
	
	public DoubleJumpingState(float speed){
		jumpSpeed = speed;
	}
	
	protected override void initializeState(){
		stateName = "doubleJumping";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
			setXVelocity(controller.getRigidbody().velocity.x);
		if(controller.useDoubleJump())
			setYVelocity(10f);
		}
}
