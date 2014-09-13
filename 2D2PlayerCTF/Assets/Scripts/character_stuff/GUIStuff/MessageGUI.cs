using UnityEngine;
using System.Collections;

public class MessageGUI : Photon.MonoBehaviour {

	private string message = "";

	public MessageGUI(){

	}


	public void updateMessage(PlayerController controller){
	
		if(controller.getHasMessage()){
			GUI.SetNextControlName("MyTextField");
			message = GUI.TextField(new Rect(10,Screen.height - 30,400,20), message, 25);
			GUI.FocusControl("MyTextField");
			if(Event.current.keyCode == KeyCode.Return){
				controller.sendMessage(message);
				message = "";
				controller.setHasMessage(false);

			}
			
		}

	}



}
