using UnityEngine;
using System.Collections;

public class WalkingService : AbstractStateService {

	public WalkingService(){
		serviceName = "walking";	
		state = new WalkingState();
	}

	protected override void initalizeService(){

	}

	public override bool checkEnterState(PlayerController controller){

		if(controller.isGrounded()){
			if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && !Input.GetKey(KeyCode.LeftShift)){
				return true;
			} else 
				return false;
		}
		return false;
	}
	public override string checkExitState(PlayerController controller){
		if(controller.isGrounded()){
			if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
				return "idle";
			else
				return null;
		}
		return "jumpingWalk";
		
	}
}
