using UnityEngine;
using System.Collections;

public class PlayerControllerGUIManager : MonoBehaviour {

	private MessageGUI messagegui = new MessageGUI();

	public void updateGUI(PlayerController controller){
		messagegui.updateMessage(controller);



	}
}
