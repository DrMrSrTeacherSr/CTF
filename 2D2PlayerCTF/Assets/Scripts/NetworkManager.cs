using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {

    private const string roomName = "Test Room";
    private RoomInfo[] roomsList;
    public GameObject playerPrefab;
	public string input = "Enter Room Name Here";
	private bool createServer = false;
	private bool joinRoom = false;



	// Use this for initialization
	void Start (){
		PhotonNetwork.sendRate = 66; //Play with this and navmeshs
		PhotonNetwork.sendRateOnSerialize = 66;
        PhotonNetwork.ConnectUsingSettings("0.1"); //Version number of the game
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	//[RPC] void displayText(String str){
		
	//	GUI.Label(Rect(0,0,Screen.width,Screen.height),"Something");
		
	//	if(photonView.isMine){
	//		photonView.RPC ("displayText",PhotonTargets.OthersBuffered,str);
	//	}
	//}

  	void OnGUI(){

		float textFieldWidth = 200f;
		float textFieldHeight = 20f;

   	 	if (!PhotonNetwork.connected) {
        	GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
   		} else if (PhotonNetwork.room == null){

			if(!createServer && !joinRoom){
        	// Create Room
				if (GUI.Button(new Rect(Screen.width/2 - textFieldWidth/2, Screen.height/3 + textFieldHeight, textFieldWidth, 50), "Start Game"))
					joinRoom = true;
				//PhotonNetwork.CreateRoom(input, true, true, 4);
 
				if (GUI.Button(new Rect(Screen.width/2 - textFieldWidth/2, Screen.height/3 + textFieldHeight + 60, textFieldWidth, 50), "Start Server"))
					createServer = true;
			} else if(joinRoom){
				input = GUI.TextField (new Rect (Screen.width/2 - textFieldWidth/2, Screen.height/3 - textFieldHeight, textFieldWidth, 20), input, 25);
				if (GUI.Button(new Rect(Screen.width/2 - textFieldWidth/2, Screen.height/3 + textFieldHeight, textFieldWidth, 50), "Join Room"))
				
				for (int i = 0; i < roomsList.Length; i++){
					if (input.Equals(roomsList[i].name)){
						PhotonNetwork.JoinRoom(input);
					}
				}

				if (GUI.Button(new Rect(Screen.width/2 - textFieldWidth/2, Screen.height/3 + textFieldHeight + 60, textFieldWidth, 50), "Back"))
					joinRoom = false;
			} else if(createServer){
				input = GUI.TextField (new Rect (Screen.width/2 - textFieldWidth/2, Screen.height/3 - textFieldHeight, textFieldWidth, 20), input, 25);
				if (GUI.Button(new Rect(Screen.width/2 - textFieldWidth/2, Screen.height/3 + textFieldHeight, textFieldWidth, 50), "Create Server")){
					PhotonNetwork.CreateRoom(input, true, true, 4);
				}
				
				if (GUI.Button(new Rect(Screen.width/2 - textFieldWidth/2, Screen.height/3 + textFieldHeight + 60, textFieldWidth, 50), "Back"))
					createServer = false;
			}


		}
	}

    void OnReceivedRoomListUpdate(){
        roomsList = PhotonNetwork.GetRoomList();
    }

    void OnJoinedRoom() {
        Debug.Log("Connected to Room");
        //Spawn Player
        PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero * 5, Quaternion.identity,0);
    }

}
