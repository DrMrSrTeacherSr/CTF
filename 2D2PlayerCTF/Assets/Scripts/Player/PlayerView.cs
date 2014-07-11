using UnityEngine;
using System.Collections;

public class PlayerView : Photon.MonoBehaviour {

	Animator anim;


	public string textToDisplay = "";
	public bool displayTheText = false;
	
	public float displayTextTimer = 10f;
	public float displayTextCount = 0f;
	
	public bool cheatText = false;
	
	string cheatCode;
	public string cheatField = "";
	public string[] pastComments;

	void Start(){
		anim = GetComponent<Animator>();
	}

	void OnGUI(){
		if(cheatText && photonView.isMine){
			GUI.SetNextControlName("MyTextField");
			cheatField = GUI.TextField(new Rect(10,Screen.height - 30,400,20), cheatField, 25);
			if(cheatField != "" && Event.current.keyCode == KeyCode.Return)
			{
				cheatCode = cheatField;
				cheatField = "";
				displayTheText = true;
				//displayText(cheatCode);
				cheatText = false;
				displayTextCount = 0f;
			}
			GUI.FocusControl("MyTextField");
		}
		if(displayTheText){
			GUI.Label(new Rect(10,Screen.height - 60,400,30),textToDisplay);
		}
		if( displayTextTimer >= displayTextCount)
			displayTextCount += 1 * Time.deltaTime;
		else {
			displayTheText= false;
			textToDisplay = "";
		}
		
	} 

	public void updateFloatAnimation(string name, float num){
		anim.SetFloat(name,num);
	}

	public void updateBoolAnimation(string name, bool boolean){
		anim.SetBool(name,boolean);
	}


}
