using UnityEngine;
using System.Collections;

public class SlidingService : AbstractStateService {

	public SlidingService(){
		serviceName = "sliding";	
		state = new SlidingState();
	}
	
	protected override void initalizeService(){
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(Input.GetKeyDown(KeyCode.S)){
			state.getTimer().start();
			return true;
		}
		return false;
	}
	
	public override string checkExitState(PlayerController controller){
		if(controller.isGrounded()){

			float percent = state.getTimer().getPercentDone();

			if(percent >= .33f && Input.GetKeyUp(KeyCode.S)){
				if(percent >= .75f){
					if(controller.isRoof())
						return "crouching";
					return "idle";
				} else if(percent >= .375f){
					if(controller.isRoof())
						return "crouching";
					return "walking";
				}else if(percent >= .33f){
					if(controller.isRoof())
						return "crouching";
					return "dashing";
				}
			}
			if(state.getTimer().isDone()){
				return "crouching";
			}
			return null;
		}else 
			return "jumpingDash";
		
	}
}
