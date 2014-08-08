using UnityEngine;
using System.Collections;

public class SneakService : AbstractStateService {

	public SneakService(){
		serviceName = "sneaking";
		state = new SneakState();
	}
	
	protected override void initalizeService(){
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(controller.isGrounded()){
			if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && Input.GetKey(KeyCode.LeftShift)){
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
			else if(!Input.GetKey(KeyCode.LeftShift))
				return "walking";
		} else 
			return "jumpingWalk";
		return null;
	}

}
