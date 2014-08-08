using UnityEngine;
using System.Collections;

public class PhotonPlayerStateService : MonoBehaviour, IPlayerStateService {
	PhotonStream stream;

	public PhotonPlayerStateService (PhotonStream stream){
		this.stream = stream;
	}

	public void loadInfo(){

	}
}

