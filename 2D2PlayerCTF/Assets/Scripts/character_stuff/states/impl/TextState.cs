using UnityEngine;
using System.Collections;

public class TextState : AbstractPlayerState {

	string textField = "";
	Timer textTimer = new Timer(5f);

	public TextState(){
		
	}
	
	protected override void initializeState(){
		stateName = "text";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		//OnGUI();

	}

	public void OnGUI(){
		Debug.Log("Also Test");
		textField = GUI.TextField(new Rect(10,Screen.height - 30,400,20), "Test", 25);

		/*
		GUI.SetNextControlName("MyTextField");
		textField = GUI.TextField(new Rect(10,Screen.height - 30,400,20), textField, 25);
		if(textField != "" && Event.current.keyCode == KeyCode.Return)
			{
				cheatCode = cheatField;
				cheatField = "";
				displayTheText = true;
				displayText(cheatCode);
				cheatText = false;
				displayTextCount = 0f;
			}
		GUI.FocusControl("MyTextField");

		if(displayTheText){
			GUI.Label(new Rect(10,Screen.height - 60,400,30),textToDisplay);
		}
		if( displayTextTimer >= displayTextCount)
			displayTextCount += 1 * Time.deltaTime;
		else {
			displayTheText= false;
			textToDisplay = "";
		}
		*/
	} 
	/*
	[RPC] void displayText(string str){
		if(str[0] != '/'){
			displayTheText = true;
			textToDisplay = str;
			textTimer.start();
			if(photonView.isMine){
				photonView.RPC ("displayText",PhotonTargets.OthersBuffered,str);
			}
		}
	}
	*/

}
