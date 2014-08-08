using UnityEngine;
using System.Collections;

public interface IPlayerState{



	void updateState(PlayerController controller);
	string getName();
	Timer getTimer();
}
