﻿using UnityEngine;
using System.Collections;

public class HangingService : AbstractStateService {

	public HangingService(){
		serviceName = "hanging";	
		state = new HangingState();
	}
	
	protected override void initalizeService(){
		
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(!controller.isGrounded() && controller.isPlayerHanging() && !Input.GetKey(KeyCode.S)){
			controller.getRigidbody().gravityScale = 0f;
			return true;
		}
		return false;
		
	}
	
	public override string checkExitState(PlayerController controller){
		if(Input.GetKey(KeyCode.S)){
			controller.getRigidbody().gravityScale = 1f;
			return "jumpingWalk";

		}
		if(!controller.isPlayerHanging()){
			controller.getRigidbody().gravityScale = 1f;
			return "jumpingWalk";
			
		}



		return null;
	}
}
