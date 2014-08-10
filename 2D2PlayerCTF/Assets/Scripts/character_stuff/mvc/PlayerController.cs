using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	private ServiceLocator locator;
	private IPlayerState currentState;
	private PlayerModel model;

	private bool facingRight = true;

	//Base Collisions 
	private bool roof = false;
	private bool grounded = false;
	private bool backWall = false;
	private bool frontWall = false;
	private bool hangCheck = false;
	private bool wallHanging = false;
	private bool onLadder = false;
	private bool doubleJumped = false;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	private float goundRadius = 0.1f;



	void Start () {
		model = GetComponent<PlayerModel>();
		locator = new ServiceLocator();
	}

	void Update () {
		checkForBaseCollisions();

		locator.updateLocator(this);

		currentState = locator.getCurrentState();
		print (currentState.getName());

		currentState.updateState(this);

		if(rigidbody2D.velocity.x < 0 && facingRight){
			model.flip();
			facingRight = !facingRight;
		} else if(rigidbody2D.velocity.x > 0 && !facingRight){
			model.flip();
			facingRight = !facingRight;
		}


		updateModel();

	}

	private void checkForBaseCollisions ()
	{
		//roof = Physics2D.OverlapCircle (roofCheck.position, shortRadius, whatIsGround);
		grounded = Physics2D.OverlapCircle (groundCheck.position, goundRadius / 2, whatIsGround);
		//backWall = Physics2D.OverlapCircle (leftWallCheck.position, shortRadius / 10, whatIsGround);
		//frontWall = Physics2D.OverlapCircle (rightWallCheck.position, shortRadius / 10, whatIsGround) || Physics2D.OverlapPoint (rightWallHangCheck2.position, whatIsGround);
		//hangCheck = Physics2D.OverlapPoint (rightWallHangCheck2.position, whatIsGround);
		//wallHanging = !Physics2D.OverlapPoint (rightWallHangCheck.position, whatIsGround) && hangCheck && frontWall && !Input.GetKey (KeyCode.S);
		if(grounded){
			doubleJumped = false;
		}
	}

	private void updateModel()
	{
		model.setCurrentState(currentState.getName());
		model.setGrounded(grounded);
		model.setOnLadder(onLadder);
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

	public bool isFrontWall(){
		return frontWall;
	}

	public bool isRoof(){
		return roof;
	}

	public bool isGrounded(){
		return grounded;
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

