using UnityEngine;
using System.Collections;

public class HangingState : AbstractPlayerState {
	

	public HangingState(){
		
	}
	
	
	protected override void initializeState(){
		stateName = "hanging";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		setYVelocity(0f);
		setXVelocity(0f);
		//setGravity(0f);
		
	}
}
