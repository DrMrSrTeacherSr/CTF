using UnityEngine;
using System.Collections;

public class ViewGUI : Photon.MonoBehaviour {

	private Timer textTimer;
	private string message;
	private PlayerModel model;
	private PhotonView photon;

	public ViewGUI(PhotonView photon, PlayerModel model){
		textTimer = new Timer(7f);
		this.model = model;
		this.photon = photon;
	}

	public void updateGUI(){
		string tempMessage = model.getMessage();

		if(tempMessage != null){
			displayText(tempMessage);
		}

		if(!textTimer.isDone()){
			displayMessage();
			textTimer.tick();
		}

	}

	private void displayMessage(){
		GUI.Label(new Rect(10,Screen.height - 60,400,30),message);
	}
	private void messageIncomming(string str){
		if(str[0] != '/'){
			message = str;
			textTimer.start ();
			//if(photon.isMine){
			//	photon.RPC ("displayText",PhotonTargets.OthersBuffered,str);
			//}
		}
	}

	[RPC] void displayText(string str){
		messageIncomming(str);
	}
}
