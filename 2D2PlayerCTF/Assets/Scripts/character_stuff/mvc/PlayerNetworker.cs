using UnityEngine;
using System.Collections;

public class PlayerNetworker : Photon.MonoBehaviour {

	private PlayerModel model;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	private bool facingRight = true;

	void Start(){
		model  = GetComponent<PlayerModel>();
	}

	void Update()
	{

		if (!photonView.isMine)
		{
			SyncedMovement();

		}
	}

	private void SyncedMovement()
	{

		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		
		if (stream.isWriting){
			//stream.SendNext(model.getPosition());
			stream.SendNext(transform.position);
			stream.SendNext(model.getXVelocity());
			stream.SendNext(model.getYVelocity());
			stream.SendNext(model.getCurrentState());
			stream.SendNext(model.getGrounded());
			stream.SendNext(model.getOnLadder());
			stream.SendNext(model.getMessage());
			//stream.SendNext(move);
			//stream.SendNext(grounded);
			//stream.SendNext(crouching);
			//stream.SendNext(dashing);
			
		}else {
			PhotonPlayerStateService service = new PhotonPlayerStateService(stream);
			//syncEndPosition = (Vector3)stream.ReceiveNext();


			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			//transform.position = (Vector3)stream.ReceiveNext();

			float xVel = (float)stream.ReceiveNext();
			float yVel = (float)stream.ReceiveNext();
			//otherMove = (float)stream.ReceiveNext();
			//otherGrounded = (bool)stream.ReceiveNext();
			//otherCrouch = (bool)stream.ReceiveNext();
			//otherDash = (bool)stream.ReceiveNext();

			model.setCurrentState((int)stream.ReceiveNext());
			model.setGrounded((bool)stream.ReceiveNext());
			model.setOnLadder((bool)stream.ReceiveNext());
			model.setYVelocity(yVel);
			model.setMessage((string)stream.ReceiveNext());
			//otherVSpeed = syncVelocity.y;
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition.x = syncPosition.x + xVel * syncDelay;
			syncEndPosition.y = syncPosition.y + yVel * syncDelay;

			//print (syncEndPosition);

			//syncStartPosition = model.getPosition();
			
			syncStartPosition = transform.position;

			
			if(xVel < -.1 && facingRight){
				model.flip();
				facingRight = !facingRight;
				//print ("flip");
			} else if(xVel > .1 && !facingRight){
				model.flip();
				facingRight = !facingRight;
				
			}
			
		}
		
	}
}
