using UnityEngine;
using System.Collections;

public class CrouchingService : AbstractStateService {

	public CrouchingService(){
		serviceName = "crouching";
		state = new CrouchingState();
	}
	
	protected override void initalizeService(){

	}
	
	public override bool checkEnterState(PlayerController controller){
		if(controller.isGrounded()){
			if(Input.GetKeyDown(KeyCode.S)){
				return true;
			}
			return false;
		}
		return false;
	}


	public override string checkExitState(PlayerController controller){


		if(controller.isGrounded() &&  !Input.GetKey(KeyCode.S) && !controller.isRoof()){
			return "idle";
		}
		return null;
	}

}
