using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServiceLocator : MonoBehaviour {

	Dictionary<string,IStateService> allServices = new Dictionary<string,IStateService>();
	ArrayList nextStates;
	IStateService currentService;

	public ServiceLocator(){
		ServiceBuilder builder = new ServiceBuilder();
		allServices = builder.getServices();

		nextStates = allServices["idle"].getNextState();
		currentService = allServices["idle"];
	}

	public IPlayerState getCurrentState(){
		return currentService.getState ();
	}

	public void updateLocator(PlayerController controller){
		nextStates = currentService.getNextState();
		string possibleNextState = currentService.checkExitState(controller);

	
		foreach(string str in nextStates){
			//print ("Update: " + str);
				if(allServices[str].checkEnterState(controller)){
					currentService = allServices[str];
				}
		} 

		if(possibleNextState != null){
				currentService = allServices[possibleNextState];
				//nextStates = currentService.getNextState();
		}


	}



}
