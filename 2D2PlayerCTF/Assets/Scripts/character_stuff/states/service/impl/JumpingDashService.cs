using UnityEngine;
using System.Collections;

public class JumpingDashService : AbstractStateService {

	public JumpingDashService(){
		serviceName = "jumpingDash";
		state = new JumpingState(8f);
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
					return "dashing";
			return "idle";
		}
		return null;
	}
}
