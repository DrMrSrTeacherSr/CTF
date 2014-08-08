using UnityEngine;
using System.Collections;


public interface IStateService{

	ArrayList getNextState();

	IPlayerState getState();
	bool checkEnterState(PlayerController controller);
	string checkExitState(PlayerController controller);
	void addNext(string str);

	
}
