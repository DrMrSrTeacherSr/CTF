using UnityEngine;
using System.Collections;

public class WallSlidingState : AbstractPlayerState {
	
	public WallSlidingState(){
		
	}
	
	
	protected override void initializeState(){
		stateName = "wallSliding";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		if(controller.getRigidbody().velocity.y < 0){
			controller.getRigidbody().gravityScale = .1f;
		} else {
			controller.getRigidbody().gravityScale = 1f;
		}
		setXVelocity(0f);
		
	}
}
