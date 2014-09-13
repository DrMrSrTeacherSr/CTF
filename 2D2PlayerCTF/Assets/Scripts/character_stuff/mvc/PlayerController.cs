using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Photon.MonoBehaviour {

	private ServiceLocator locator;
	private IPlayerState currentState;
	private PlayerModel model;

	private bool facingRight = true;

	//Base Collisions 
	private bool roof = false;
	private bool grounded = false;
	private bool backWall = false;
	private bool frontWall = false;
	private bool hangingCheck = false;
	private bool isHanging = false;
	private bool wallHanging = false;
	private bool onLadder = false;
	private bool doubleJumped = false;
	private bool onWall = false;
	public bool wall1Check;
	public bool wall2Check;
	public bool wall3Check;
	public Transform groundCheck;
	public Transform roofCheck;
	public Transform wall1;
	public Transform wall2;
	public Transform wall3;
	public Transform hangCheck;
	public LayerMask whatIsGround;
	private float goundRadius = 0.1f;

	private bool hasMessage = false;

	public BoxCollider2D collider;

	private int currentStateID = 0;

	public bool alwaysDash = true;

	private PlayerControllerGUIManager guiManager;



	void Start () {
		model = GetComponent<PlayerModel>();
		locator = new ServiceLocator();
		guiManager = new PlayerControllerGUIManager();
	}

	void OnGUI(){
		guiManager.updateGUI(this);
	}

	void Update () {
		checkForBaseCollisions();


		locator.updateLocator(this);

		currentState = locator.getCurrentState();

		//print (currentState.getName());
//		print (getRigidbody().gravityScale);

		//currentState.updateState(this);

		if(rigidbody2D.velocity.x < -.1 && facingRight){
			model.flip();
			facingRight = !facingRight;
			//print ("flip");
		} else if(rigidbody2D.velocity.x > .1 && !facingRight){
			model.flip();
			facingRight = !facingRight;

		}
		currentStateIDUpdate();
		adjustCollisionBox();
		updateModel();

	}

	private void checkForBaseCollisions ()
	{
		//roof = Physics2D.OverlapCircle (roofCheck.position, shortRadius, whatIsGround);
		grounded = Physics2D.OverlapCircle (groundCheck.position, goundRadius / 2, whatIsGround);
		//backWall = Physics2D.OverlapCircle (leftWallCheck.position, shortRadius / 10, whatIsGround);

		//Vector2 startPosition = rightWallCheck.position;
		//Vector2 endPosition = new Vector2(startPosition.x + .1f,startPosition.y - .25f);


		//Debug.DrawLine(new Vector3(startPosition.x,startPosition.y,0),new Vector3(endPosition.x,endPosition.y,0));


		//frontWall = Physics2D.OverlapArea (startPosition,endPosition, whatIsGround);// || Physics2D.OverlapPoint (rightWallHangCheck2.position, whatIsGround);
		wall1Check = Physics2D.OverlapPoint (wall1.position, whatIsGround);
		wall2Check = Physics2D.OverlapPoint (wall2.position, whatIsGround);
		wall3Check = Physics2D.OverlapPoint (wall3.position, whatIsGround);
		roof = Physics2D.OverlapPoint (roofCheck.position, whatIsGround);
		hangingCheck = Physics2D.OverlapPoint (hangCheck.position, whatIsGround);

		//wallHanging = !Physics2D.OverlapPoint (rightWallHangCheck.position, whatIsGround) && hangCheck && frontWall && !Input.GetKey (KeyCode.S);
		//print ("wall: " + (wall1Check && wall2Check));
		//print ("hang: " + hangingCheck);
		isHanging = (wall1Check && wall2Check && !hangingCheck);
		onWall = wall1Check || wall2Check || wall3Check;
		if(grounded || onWall){
			doubleJumped = false;
		}
	}

	private void updateModel()
	{
		model.setCurrentState(currentStateID);
		model.setGrounded(grounded);
		model.setOnLadder(onLadder);
		//model.setHanging(isHanging);
		model.setXVelocity(rigidbody2D.velocity.x);
		model.setYVelocity(rigidbody2D.velocity.y);
		/*
		model.set("grounded",grounded);
		model.set("crouching",crouching);
		model.set("dashing",dashing);
		model.set("sneaking",sneaking);
		model.set("doubleJump",doubleJump);
		model.set("sliding",sliding);
		model.set("wallSliding",wallSliding);
		model.set("climbing",climbing);
		model.set("onLadder",onLadder);
		model.set("hanging",wallHanging);
		
		model.set("facingRight",facingRight);
		model.setPosition(rigidbody2D.position);
		
		model.setXVelocity(rigidbody2D.velocity.x);
		model.setYVelocity(rigidbody2D.velocity.y);
		*/
	}

	private void adjustCollisionBox ()
	{
		if(currentState.getName().Equals("crouching")||currentState.getName().Equals("sliding")||currentState.getName().Equals("crawling")){
			collider.center = new Vector2(collider.center.x,-.17f);
			collider.size = new Vector2(.43f,.23f);
//			roofCheck.localPosition = new Vector2(roofCheck.localPosition.x,.14f);
		}else {
			collider.center = new Vector2(collider.center.x,.02f);
			collider.size = new Vector2(.31f,.56f);
//			roofCheck.localPosition = new Vector2(roofCheck.localPosition.x,.437f);
		}
	}


	private void currentStateIDUpdate (){
		switch(currentState.getName()){
		
		case "jumping" : currentStateID = -1; break;
		case "idle" : currentStateID = 0; break;
		case "walking" : currentStateID = 1; if(alwaysDash) currentStateID = 2; break;
		case "dashing" : currentStateID = 2;break;
		case "crouching" : currentStateID = 3;break;
		case "crawling" : currentStateID = 4;break;
		case "sneaking" : currentStateID = 5;break;
		case "sliding" : currentStateID = 6;break;
		case "onLadder" : currentStateID = 7;break;
		case "climbing" : currentStateID = 8;break;
		case "doubleJumping" : currentStateID = 9;break;
		case "hanging" : currentStateID = 10;break;
		case "wallSliding" : currentStateID = 11;break;
		case "text" : currentStateID = 12;break;
		}
	}



























	public void sendMessage(string message){
	
		model.setMessage(message);
	}

	public PhotonView getPhotonView(){
		return photonView;
	}
	
	public void setHasMessage(bool boolean){
		hasMessage = boolean;
	}

	public bool getHasMessage(){
		return hasMessage;
	}

	public bool isFrontWall(){
		return frontWall;
	}

	public bool isRoof(){
		return roof;
	}

	public bool isGrounded(){
		return grounded;
	}

	public bool isPlayerHanging(){
		return isHanging;
	}

	public bool isOnWall(){
		return onWall;
	}

	public bool isFacingRight(){
		return facingRight;
	}

	public bool isDoubleJumpAvailable(){
		return !doubleJumped;
	}

	public bool useDoubleJump(){
		if (!doubleJumped){
			doubleJumped = true;
			return true;
		}
		return false;
	}

	public bool isOnLadder(){
		return onLadder;
	}

	public Rigidbody2D getRigidbody(){
		return rigidbody2D;
	}


	public IPlayerState getCurrentState(){
		return currentState;
	}

	public void OnTriggerStay2D (Collider2D col)
	{
		
		if (col.gameObject.tag == "Ladder") {
			onLadder = true;
		}
	}
	
	public void OnTriggerExit2D (Collider2D col)
	{
		if (col.gameObject.tag == "Ladder") {
			onLadder = false;
		}
	}


}

