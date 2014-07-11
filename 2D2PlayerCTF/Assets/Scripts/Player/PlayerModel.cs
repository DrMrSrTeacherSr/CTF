using UnityEngine;
using System.Collections;

public class PlayerModel : Photon.MonoBehaviour {

	public PlayerView view;

	public float maxSpeed = 2f;
	public float moving;
	public bool grounded = false;
	public bool crouching = false;
	public bool dashing = false;
	public bool facingRight = true;



	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	
	private float otherMove;
	private float otherVSpeed;
	private bool otherGrounded; //Condense into 0101010101
	private bool otherCrouch;
	private bool otherDash;

	void Update(){
		
		view.updateFloatAnimation("vSpeed",rigidbody2D.velocity.y);
		view.updateBoolAnimation("Crouch",crouching);
		view.updateFloatAnimation("Speed", Mathf.Abs(moving));
		view.updateBoolAnimation("Dashing",dashing);
		view.updateBoolAnimation("Ground",grounded);
	}

	private void SyncedMovement(){
		
		if(otherMove > 0 && !facingRight){
			//flip();
		} else if(otherMove < 0 && facingRight){
		//	flip();
		}
		
		//set animations

		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(syncStartPosition,syncEndPosition,syncTime/syncDelay);
	}
	
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		
		if (stream.isWriting){
			stream.SendNext(transform.position);
			stream.SendNext(rigidbody2D.velocity);
			stream.SendNext(moving);
			stream.SendNext(grounded);
			stream.SendNext(crouching);
			stream.SendNext(dashing);
			
		}else {
			
			//syncEndPosition = (Vector3)stream.ReceiveNext();
			
			Vector3 syncPosition = (Vector3)stream.ReceiveNext();
			Vector2 syncVelocity = (Vector2)stream.ReceiveNext();
			otherMove = (float)stream.ReceiveNext();
			otherGrounded = (bool)stream.ReceiveNext();
			otherCrouch = (bool)stream.ReceiveNext();
			otherDash = (bool)stream.ReceiveNext();
			
			otherVSpeed = syncVelocity.y;
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition.x = syncPosition.x + syncVelocity.x * syncDelay;
			syncEndPosition.y = syncPosition.y + syncVelocity.y * syncDelay;
			//syncStartPosition = rigidbody2D.position;
			
			syncStartPosition = transform.position;
			
		}
		
	}

	[RPC] void ChangeColorTo(Vector3 color){
		renderer.material.color = new Color(color.x,color.y,color.z,1f);
		if(photonView.isMine){
			photonView.RPC ("ChangeColorTo",PhotonTargets.OthersBuffered,color);
		}
	}
	
	[RPC] void displayText(string str){
		if(str[0] != '/'){
			view.displayTheText = true;
			view.textToDisplay = str;
			view.displayTextCount = 0f;
			if(photonView.isMine){
				photonView.RPC ("displayText",PhotonTargets.OthersBuffered,str);
			}
		}
	}
}
