using UnityEngine;
using System.Collections;

public class DoubleJumpingWalkService : AbstractStateService {

	public DoubleJumpingWalkService(){
		serviceName = "doubleJumpingWalk";
		state = new DoubleJumpingState(4f);
	}
	
	protected override void initalizeService(){
		
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(Input.GetKeyDown(KeyCode.W) && !controller.isGrounded() && controller.isDoubleJumpAvailable()){
			return true;
		}
		return false;
		
	}

	public override string checkExitState(PlayerController controller){
		if(!Input.GetKeyDown(KeyCode.W) && controller.isGrounded()){
			if(Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
				return "walking";
			return "idle";
		}
		return null;
	}

}
