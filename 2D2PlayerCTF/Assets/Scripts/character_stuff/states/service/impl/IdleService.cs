using UnityEngine;
using System.Collections;

public class IdleService : AbstractStateService {

	public IdleService(){
		serviceName = "idle";
		state = new IdleState();
	}

	protected override void initalizeService(){

	}

	public override bool checkEnterState(PlayerController controller){
		return false;
		
	}
	public override string checkExitState(PlayerController controller){
		return null;
	}

}
