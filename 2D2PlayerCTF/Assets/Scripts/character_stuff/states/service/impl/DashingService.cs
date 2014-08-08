using UnityEngine;
using System.Collections;

public class DashingService : AbstractStateService {

	Timer dashTimer = new Timer(0.5f);
	int dashCounter = 0;


	public DashingService(){
		serviceName = "dashing";	
		state = new DashingState();
	}
	
	protected override void initalizeService(){
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(controller.isGrounded()){
			if(Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.A)){
				if (!dashTimer.isDone() && dashCounter == 1/*Number of Taps you want Minus One*/){
					return true;
				}else{
					dashTimer.start (); 
					dashCounter += 1 ;
				}
			}
			if (dashTimer.isDone()){
				dashCounter = 0 ;
			}else{
				dashTimer.tick(); 
			}
		return false;
		}
		return false;
	}

	public override string checkExitState(PlayerController controller){
		if(controller.isGrounded()){
		dashTimer.tick(); 
		if(Input.GetKey(KeyCode.D) && Input.GetKeyUp(KeyCode.A) || Input.GetKey(KeyCode.A) && Input.GetKeyUp(KeyCode.D))
			return "walking";
		if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			return "idle";
		else
			return null;
		}
		return "jumpingDash";
		
	}
}
