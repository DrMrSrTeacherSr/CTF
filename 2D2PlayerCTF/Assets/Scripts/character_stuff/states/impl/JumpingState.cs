using UnityEngine;
using System.Collections;

public class JumpingState : AbstractPlayerState{

	private float jumpSpeed;

	public JumpingState(float speed){
		jumpSpeed = speed;
	}
	
	protected override void initializeState(){
		stateName = "jumping";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		if(Input.GetKey (KeyCode.A))
			setXVelocity(jumpSpeed * Input.GetAxis("Horizontal"));
		else if(Input.GetKey (KeyCode.D))
			setXVelocity(jumpSpeed* Input.GetAxis("Horizontal"));
		if(controller.isGrounded()){
			setYVelocity(15f);
		}
	}


}
