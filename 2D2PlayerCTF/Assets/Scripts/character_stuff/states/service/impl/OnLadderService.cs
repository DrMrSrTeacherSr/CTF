﻿using UnityEngine;
using System.Collections;

public class OnLadderService : AbstractStateService {

	public OnLadderService(){
		serviceName = "onLadder";	
		state = new OnLadderState();
	}
	
	protected override void initalizeService(){
		
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(controller.isOnLadder() && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))){
			controller.getRigidbody().gravityScale = 0f;
			return true;
		}
		return false;
		
	}

	public override string checkExitState(PlayerController controller){

		if(!controller.isOnLadder() && controller.isGrounded()){
			controller.getRigidbody().gravityScale = 1f;
			return "idle";
		}else if(!controller.isOnLadder() && !controller.isGrounded()){
			controller.getRigidbody().gravityScale = 1f;
			return "jumpingWalk";
		}
		return null;
		
	}
}
