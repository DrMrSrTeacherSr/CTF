using UnityEngine;
using System.Collections;

public class CrouchingState : AbstractPlayerState {

	public CrouchingState(){
		
	}
	
	protected override void initializeState(){
		stateName = "crouching";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
	}

}
