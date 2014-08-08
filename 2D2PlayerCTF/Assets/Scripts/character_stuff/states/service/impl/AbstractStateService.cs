using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractStateService : MonoBehaviour, IStateService {

	protected ArrayList nextStates;
	protected IPlayerState state;
	protected string serviceName;

	public AbstractStateService(){
		nextStates = new ArrayList();
		initalizeService();
	}

	protected abstract void initalizeService();
	public abstract bool checkEnterState(PlayerController controller);
	public abstract string checkExitState(PlayerController controller);

	public void addNext(string str){
		nextStates.Add(str);
	}

	public IPlayerState getState(){
		return state;
	}

	public ArrayList getNextState(){
		return nextStates;
	}

}
