using UnityEngine;
using System.Collections;

public class WallSlidingService : AbstractStateService {

	public WallSlidingService(){
		serviceName = "wallSliding";	
		state = new WallSlidingState();
	}
	
	protected override void initalizeService(){
		
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(controller.isOnWall()){
			return true;
		}
		return false;
		
	}
	
	public override string checkExitState(PlayerController controller){
		if(controller.isOnWall() && Input.GetKeyDown(KeyCode.W)){
			controller.getRigidbody().gravityScale = 1f;
			if(controller.isFacingRight()){
				controller.getRigidbody().velocity = new Vector2(-8f,controller.getRigidbody().velocity.y);
			}else{
				controller.getRigidbody().velocity = new Vector2(8f,controller.getRigidbody().velocity.y);
			}
			return "doubleJumpingWalk";

		}

		if(controller.isGrounded()){
			controller.getRigidbody().gravityScale = 1f;
			return "idle";
		}

		if(!controller.isOnWall()){
			controller.getRigidbody().gravityScale = 1f;
			return "jumpingWalk";
		}

		return null;
		
	}
}
