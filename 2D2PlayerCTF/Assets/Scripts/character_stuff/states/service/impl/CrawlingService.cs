using UnityEngine;
using System.Collections;

public class CrawlingService : AbstractStateService {

	public CrawlingService(){
		serviceName = "crawling";
		state = new CrawlingState();
	}

	protected override void initalizeService(){

	}

	public override bool checkEnterState(PlayerController controller){
		if(controller.isGrounded()){
			if((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S)){
				return true;
			}
		}
		return false;

	}
	public override string checkExitState(PlayerController controller){
		if(controller.isGrounded()){
			if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
				return "crouching";
			else if (!Input.GetKey(KeyCode.S))
				return "walking";
			else
				return null;
		}
		return "jumpingWalk";	
	}

}
	
