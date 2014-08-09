using UnityEngine;
using System.Collections;

public class ClimbingService : AbstractStateService {

	public ClimbingService(){
		serviceName = "climbing";	
		state = new ClimbingState();
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
		if(!controller.isOnLadder() && !controller.isGrounded()){
			controller.getRigidbody().gravityScale = 1f;
			return "jumpingWalk";
		}
		if(!controller.isOnLadder() && controller.isGrounded()){
			controller.getRigidbody().gravityScale = 1f;
			return "idle";
		}
		if(controller.isOnLadder() && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)){
			return "onLadder";
		}


		return null;
		
	}
}
