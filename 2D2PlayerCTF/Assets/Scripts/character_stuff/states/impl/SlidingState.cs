using UnityEngine;
using System.Collections;

public class SlidingState : AbstractPlayerState {


	private float slidingSpeed = 12f;
	
	
	public SlidingState(){
		stateTimer = new Timer(1.5f);
	}
	
	protected override void initializeState(){
		stateName = "sliding";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;

		Vector2 slidingVector = controller.getRigidbody().velocity;
		slidingVector.x = slidingSpeed * (1 - stateTimer.getPercentDone());
		if(controller.getRigidbody().velocity.x < 0)
			slidingVector.x *= -1;
		controller.getRigidbody().velocity = slidingVector;

		stateTimer.tick ();
	}

}
