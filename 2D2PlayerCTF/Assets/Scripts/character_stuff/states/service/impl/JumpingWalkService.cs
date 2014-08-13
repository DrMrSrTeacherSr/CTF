using UnityEngine;
using System.Collections;

public class JumpingWalkService : AbstractStateService {

	public JumpingWalkService(){
		serviceName = "jumpingWalk";
		state = new JumpingState(4f);
	}
	
	protected override void initalizeService(){

	}
	
	public override bool checkEnterState(PlayerController controller){
		if(Input.GetKeyDown(KeyCode.W) && controller.isGrounded()){
			controller.getRigidbody().gravityScale = 1f;
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
