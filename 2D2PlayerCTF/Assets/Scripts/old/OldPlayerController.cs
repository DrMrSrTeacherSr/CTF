using UnityEngine;
using System.Collections;

public class OldPlayerController : Photon.MonoBehaviour
{

	PlayerModel model;
	private bool facingRight = true;
	private float horizontalMovement = 0f;
	private float verticalMovement = 0f;

	public BoxCollider2D collider;

	//MaxSpeeds
	private float maxSpeed = 2f;
	private float slidingSpeed = 10f;
	private float runSpeed = 6f;
	private float walkSpeed = 2f;
	private float crawlSpeed = 1f;

	//Base Collisions 
	private bool roof = false;
	private bool grounded = false;
	private bool backWall = false;
	private bool frontWall = false;
	private bool hangCheck = false;
	private bool wallHanging = false;

	//Text
	private bool isText = false;

	//Dashing
	private bool dashing = false;

	private float dashTimer = 0.5f;
	private float dashCounter = 0f;

	//Crawling
	private bool crouching = false;

	//Sliding

	private bool sliding = false;
	private bool slidingShort = false;
	private float slideTimer = 2f;
	private float slideCounter = 0f;

	//Sneaking
	private bool sneaking = false;

	//Jumping
	private bool jumped = false;
	private bool doubleJump = true;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	private float goundRadius = 0.1f;

	//Ladder
	private bool onLadder = false;
	private bool climbing = false;

	//Wall
	private bool wallSliding = false;
	private bool wallJumped = false;
	public Transform leftWallCheck;
	public Transform rightWallCheck;
	public Transform rightWallHangCheck2;
	public Transform rightWallHangCheck;
	private float shortRadius = 0.2f;
	private bool upFromWallHang  = false;

	//Roof
	public Transform roofCheck;

	void Start ()
	{
		model = GetComponent<PlayerModel> ();
	}

	void Update ()
	{
		checkForBaseCollisions ();
		recieveInput ();
		adjustCollisionBox ();
		updateVariables ();


	}

	void FixedUpdate(){
		getVelocity ();
	}

	private void checkForBaseCollisions ()
	{
		roof = Physics2D.OverlapCircle (roofCheck.position, shortRadius, whatIsGround);
		grounded = Physics2D.OverlapCircle (groundCheck.position, goundRadius / 2, whatIsGround);
		backWall = Physics2D.OverlapCircle (leftWallCheck.position, shortRadius / 10, whatIsGround);
		frontWall = Physics2D.OverlapCircle (rightWallCheck.position, shortRadius / 10, whatIsGround) || Physics2D.OverlapPoint (rightWallHangCheck2.position, whatIsGround);
		hangCheck = Physics2D.OverlapPoint (rightWallHangCheck2.position, whatIsGround);
		wallHanging = !Physics2D.OverlapPoint (rightWallHangCheck.position, whatIsGround) && hangCheck && frontWall && !Input.GetKey (KeyCode.S);

		if(frontWall && !grounded && !onLadder && !wallHanging && !sliding && !slidingShort){
			wallSliding = true;
			doubleJump = false;
			wallJumped = false;
			dashing = false;
		}else if (wallHanging && !Input.GetKey(KeyCode.W)){
			doubleJump = true;
			wallJumped = false;
			wallSliding = false;
			dashing = false;
			
		}else {
			wallSliding = false;

		}
		if(!frontWall){
			wallHanging = false;
		}

		if(wallSliding)
			wallJumped = false;
		
		if(grounded){
			doubleJump = false;
			wallSliding = false;
			wallJumped = false;

			
		}


	}

	private void recieveInput(){
		checkForDashing();
		checkForSneak();
		checkForJump();
		checkForCrouch();


	}



	private void getVelocity(){
		horizontalMovement = walkSpeed;

		if(dashing){
			horizontalMovement = runSpeed;
		}
		if(sneaking||crouching){
			horizontalMovement = crawlSpeed;
		}if(sliding){
			horizontalMovement = slidingSpeed * (slideTimer - slideCounter)/slideTimer;
		}
		if(wallJumped){
			horizontalMovement = 5f;
			
		}
		float input;
		if(facingRight)
			input = 1f;
		else
			input = -1f;

		if(!sliding && !wallJumped){
			input = Input.GetAxis("Horizontal");
		}

		float moveH = input * horizontalMovement;


		if(moveH < 0 && facingRight && !wallJumped){
			facingRight = !facingRight;
			model.flip();
		}else if(moveH > 0 && !facingRight  && !wallJumped){
			facingRight = !facingRight;
			model.flip();
		}

		if(jumped){
			jumped = false;
			yVelocity(verticalMovement);
		}

		if(onLadder){
			yVelocity(verticalMovement);
		}
		

		if(wallSliding){
			moveH = 0f;
			yVelocity(-.5f);
		}


		xVelocity(moveH);
	}

	private void checkForJump(){
		
		if(!roof && !onLadder){
			if(((grounded||!doubleJump||wallSliding||wallHanging) && Input.GetKeyDown(KeyCode.W) && !sliding)){ 
				jumped = true;
				verticalMovement = 12.5f;
				grounded = false;
				
				if(wallHanging){
					//Vector3 vx = rigidbody2D.velocity;
					//vx.x = 1f;
					//rigidbody2D.velocity = vx;
					
				}
				
				if((!doubleJump && !grounded && !wallHanging) || wallSliding){
					doubleJump = true;
					if(wallSliding){
						facingRight = !facingRight;
						model.flip ();
						wallJumped = true;
						wallSliding = false;
					}
					
				}
				
				
			}
		}else if (onLadder){
			if(Input.GetKey(KeyCode.W)){
				verticalMovement = 3f;
				horizontalMovement = 0f;
				doubleJump = false;
				climbing = true;
			}else if(!climbing){
				verticalMovement = 0f;
				climbing = false;

			}
		} 
		if(wallHanging && Input.GetKey(KeyCode.W)){
			upFromWallHang = true;
		}
	}


	private void checkForCrouch(){
		if(grounded && !onLadder){
			if(grounded && Input.GetKey(KeyCode.S)){
				if(dashing && grounded && !crouching && Input.GetKeyDown(KeyCode.S)){
					dashing = false;
					sneaking = false;
					sliding = true;
					slideCounter = 0f;
				}else if(!sliding){
					dashing = false;
					sneaking = false;
					crouching = true;
				}
				
			} else {
				if(!roof){
					crouching = false;
					slidingShort = false;
				}
			}
			if(sliding && (slideCounter >= slideTimer/3 && (Input.GetKeyUp(KeyCode.S)) || (slideCounter >= slideTimer))){
				if(roof){
					crouching = true;
					sliding = false;
				}else{
					if(slideCounter >= 3*slideTimer/4){
						crouching = true;
						slidingShort = true;
					} else if(slideCounter >= 3*slideTimer/8){
						slidingShort = false;
					}else if(slideCounter >= slideTimer/3){
						dashing = true;
						slidingShort = false;
					}
					sliding = false;
				}
				
				
			} else if (sliding && slideCounter < slideTimer){
				slideCounter += 1*Time.deltaTime;
			}
			if(slideCounter > slideTimer/7 && sliding)
				slidingShort = true;
		}else if(wallSliding){
			/*if(Input.GetKey(KeyCode.S))
				verticalSpeedLimit = 1f;
			else
				verticalSpeedLimit = .5f;*/
		}else if(!onLadder){
			if(roof && grounded){
				crouching = true;
				sliding = false;
			}else{
				slidingShort = false;
				crouching = false;
				sliding = false;
			}
		}

		if(onLadder && Input.GetKey(KeyCode.S)){
			verticalMovement = -3f;
			climbing = true;

		}else if(onLadder && !climbing){
			verticalMovement = 0f;
			climbing = false;
		}
	}


	private void checkForSneak(){
		if(grounded && Input.GetKey(KeyCode.LeftShift)){
			sneaking = true;
			crouching = false;
			dashing = false;
		}else
			sneaking = false;
	}


	void checkForDashing(){
		if(dashing && (Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.A)) && dashTimer < 0){
			dashing = false;
		}		
		if(Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.A) && grounded && !crouching && !sliding){
			if ( dashTimer > 0 && dashCounter == 1/*Number of Taps you want Minus One*/){
				dashing = true;
			}else{
				dashTimer = 0.5f ; 
				dashCounter += 1 ;
			}
		}
		if ( dashTimer > 0 ){
			
			dashTimer -= 1 * Time.deltaTime ;
			
		}else{
			dashCounter = 0 ;
		}
	}

	private void adjustCollisionBox ()
	{
		if(crouching || slidingShort){
			collider.center = new Vector2(collider.center.x,-.26f);
			collider.size = new Vector2(.88f,.18f);
			roofCheck.localPosition = new Vector2(roofCheck.localPosition.x,.14f);
		}else {
			collider.center = new Vector2(collider.center.x,.1f);
			collider.size = new Vector2(.28f,.69f);
			roofCheck.localPosition = new Vector2(roofCheck.localPosition.x,.437f);
		}
	}

	public void xVelocity(float xVel){
		Vector3 v = rigidbody2D.velocity;
		v.x = xVel;
		rigidbody2D.velocity = v;
	}

	public void yVelocity(float yVel){
		Vector3 v = rigidbody2D.velocity;
		v.y = yVel;
		rigidbody2D.velocity = v;
	}

	private void updateVariables ()
	{
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

	public void OnTriggerStay2D (Collider2D col)
	{
		
		if (col.gameObject.tag == "Ladder" && !sliding) {
			onLadder = true;
			crouching = false;
			rigidbody2D.gravityScale = 0f;
		}
	}
	
	public void OnTriggerExit2D (Collider2D col)
	{
		
		if (col.gameObject.tag == "Ladder") {
			onLadder = false;
			climbing = false;
			rigidbody2D.gravityScale = 1f;
		}
	}



}
