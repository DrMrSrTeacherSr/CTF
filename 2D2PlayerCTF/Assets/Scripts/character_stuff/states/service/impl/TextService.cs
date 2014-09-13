using UnityEngine;
using System.Collections;

public class TextService : AbstractStateService {

	public TextService(){
		serviceName = "text";
		state = new TextState();
	}
	
	protected override void initalizeService(){
		
	}
	
	public override bool checkEnterState(PlayerController controller){
		if(Input.GetKeyDown(KeyCode.T)){
			controller.setHasMessage(true);
			return true;
		}

		return false;
		
	}
	public override string checkExitState(PlayerController controller){
		if(!controller.getHasMessage()){
			return "idle";
		}
		return null;
	}
}
