using UnityEngine;
using System.Collections;

public class Player : Photon.MonoBehaviour {

	float move;
	float verticalSpeedLimit = 1f;
	public bool facingRight = true;
	Animator anim;
	public BoxCollider2D collider;
	public float maxSpeed = 2f;
	public float walkSpeed = 2f;

	//Dashing
	//----------------------------------------------------------------------
	public float runSpeed = 6f;
	public bool dashing = false;
	public float dashTimer = 0.5f;
	private float dashCounter = 0f;

	//Sliding
	//----------------------------------------------------------------------
	public float slidingSpeed = 10f;
	public bool sliding = false;
	public bool slidingShort = false;
	public float slideTimer = 2f;
	private float slideCounter = 0f;

	//Crawling
	//----------------------------------------------------------------------
	public float crawlSpeed = 1f;
	public bool crouching = false;

	//Sneaking
	//----------------------------------------------------------------------
	public bool sneaking = false;

	//Jumping
	//----------------------------------------------------------------------
	bool grounded = false;
	public bool doubleJump = true;
	public Transform groundCheck;
	private float goundRadius = 0.1f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;

	//Ladder
	//----------------------------------------------------------------------
	public bool ladder = false;
	public bool climbing = false;

	//Wall
	//----------------------------------------------------------------------
	public bool backWall = false;
	public bool frontWall = false;
	public bool hangCheck = false;
	public bool wallSliding = false;
	public bool wallHanging = false;
	public bool wallJumped = false;
	public Transform leftWallCheck;
	public Transform rightWallCheck;
	public Transform rightWallHangCheck2;
	public Transform rightWallHangCheck;

	private float shortRadius = 0.2f;

	//Roof Check
	//----------------------------------------------------------------------
	public bool roof = true;
	public Transform roofCheck;

	//Syncing
	//----------------------------------------------------------------------
	private float otherMove;
	private float otherVSpeed;
	private bool otherGrounded; //Condense into 0101110101
	private bool otherCrouch;
	private bool otherDash;

    private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	//Text
	//----------------------------------------------------------------------
	private bool cheatText = false;

	private string textToDisplay = "";
	private bool displayTheText = false;

	private float displayTextTimer = 10f;
	private float displayTextCount = 0f;
	private string cheatCode;
	private string cheatField = "";
	public string[] pastComments;

	//----------------------------------------------------------------------

	void Start(){
		anim = GetComponent<Animator>();
	}

	void OnGUI(){
		anim.SetBool("Text",cheatText);
		if(cheatText && photonView.isMine){
			GUI.SetNextControlName("MyTextField");
			cheatField = GUI.TextField(new Rect(10,Screen.height - 30,400,20), cheatField, 25);
			if(cheatField != "" && Event.current.keyCode == KeyCode.Return)
			{
				cheatCode = cheatField;
				cheatField = "";
				displayTheText = true;
				displayText(cheatCode);
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


	void FixedUpdate () {
        if(photonView.isMine){
       		InputMovement();
			InputColorChange();
		} else {
			SyncedMovement();
		}
	}

	//Update
	//-----------------------------------------------------------------------------------------------------------------------------------------
	void Update(){

		checkForText();
		checkForDashing();
		checkForJump();
		checkForCrouch();
		checkForSneak();


	}

	void checkForText(){
		if(Input.GetKeyUp(KeyCode.T)){
			cheatText = true;
		}
	}

	void checkForDashing(){
		if(dashing && (Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.A)) && dashTimer < 0 ){
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

	void checkForJump(){

		if(!roof && !ladder){
			if(((grounded||!doubleJump||wallSliding||wallHanging) && Input.GetKeyDown(KeyCode.W) && !sliding)){ 
				Vector3 v = rigidbody2D.velocity;
				v.y = 12.5f;
				rigidbody2D.velocity = v;
				grounded = false;

				if(wallHanging){
					Vector3 vx = rigidbody2D.velocity;
					vx.x = 1f;
					rigidbody2D.velocity = vx;

				}

				if((!doubleJump && !grounded && !wallHanging) || wallSliding){
					doubleJump = true;
					if(wallSliding){
						flip ();
						wallJumped = true;
						wallSliding = false;
					}

				}


			}
		}else if (ladder){
			if(Input.GetKey(KeyCode.W)){
				Vector3 v = rigidbody2D.velocity;
				v.y = 3f;
				climbing = true;
				rigidbody2D.velocity = v;
				Vector3 vx = rigidbody2D.velocity;
				vx.x = 0f;
				rigidbody2D.velocity = vx;
			}else{
				Vector3 v = rigidbody2D.velocity;
				v.y = 0f;
				rigidbody2D.gravityScale = 0f;
				climbing = false;
				rigidbody2D.velocity = v;
			}
		} 
	}




	void checkForCrouch(){
		if(grounded && !ladder){
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
			if(Input.GetKey(KeyCode.S))
				verticalSpeedLimit = 1f;
			else
				verticalSpeedLimit = .5f;
		}else if(!ladder){
			if(roof && grounded){
				crouching = true;
				sliding = false;
			}else{
				slidingShort = false;
				crouching = false;
				sliding = false;
			}
		} else if(ladder && Input.GetKey(KeyCode.S)){
			Vector3 v = rigidbody2D.velocity;
			v.y = -3f;
			climbing = true;
			rigidbody2D.velocity = v;
		} 
	}

	void checkForSneak(){
		if(grounded && Input.GetKey(KeyCode.LeftShift)){
		   sneaking = true;
			crouching = false;
			dashing = false;
		}else
			sneaking = false;
	}
	//-----------------------------------------------------------------------------------------------------------------------------------------


    void InputMovement(){


		roof = Physics2D.OverlapCircle(roofCheck.position, shortRadius, whatIsGround);
		grounded = Physics2D.OverlapCircle(groundCheck.position, goundRadius/2, whatIsGround);
		backWall = Physics2D.OverlapCircle(leftWallCheck.position,shortRadius/10, whatIsGround);
		frontWall = Physics2D.OverlapCircle(rightWallCheck.position,shortRadius/10, whatIsGround) || Physics2D.OverlapPoint(rightWallHangCheck2.position, whatIsGround);
		hangCheck = Physics2D.OverlapPoint(rightWallHangCheck2.position, whatIsGround);
		wallHanging = !Physics2D.OverlapPoint(rightWallHangCheck.position, whatIsGround) && hangCheck && frontWall && !Input.GetKey(KeyCode.S);



		if(frontWall && !grounded && !ladder && !wallHanging){
			wallJumped = false;
			wallSliding = true;
			doubleJump = false;
			dashing = false;
			rigidbody2D.gravityScale = 1f;
		}else if (wallHanging && !Input.GetKey(KeyCode.W)){
			verticalSpeedLimit = 0f;
			rigidbody2D.gravityScale = 0f;
			wallJumped = false;
			wallSliding = false;
			doubleJump = true;
			dashing = false;

		}else{
			wallSliding = false;
			verticalSpeedLimit = 1f;
			rigidbody2D.gravityScale = 1f;
		}
		if(!frontWall){
			wallHanging = false;
		}


		if((move > 0 && !facingRight && !wallJumped)){
			flip();
		} else if((move < 0 && facingRight && !wallJumped)){
			flip();
		}

		getMaxSpeed();

		float movement = move * maxSpeed;


		if(wallSliding)
			wallJumped = false;



		rigidbody2D.velocity = new Vector2(movement, rigidbody2D.velocity.y * verticalSpeedLimit);

		updateAnimations();
		adjustCollisionBox();

		if(grounded){
			doubleJump = false;
			wallSliding = false;
			wallJumped = false;
			
		}
    }

	void getMaxSpeed(){
		if(!doubleJump){
			if(!(sliding || dashing || crouching || sneaking))
				maxSpeed = walkSpeed;
			else {
				if(sliding)
					maxSpeed = slidingSpeed * (slideTimer - slideCounter)/slideTimer;
				if(dashing){
					maxSpeed = runSpeed;
				}
				if(crouching|| sneaking){
					maxSpeed = crawlSpeed;
				}
			}
			if(!sliding)
				move = Input.GetAxis("Horizontal");
		}
		if(wallSliding){
			move = 0f;
		}
		if(wallJumped){
			if(facingRight)
				move = 2f;
			else
				move = -2f;
		}
		if(ladder){
			move = Input.GetAxis("Horizontal");
			doubleJump = false;
		}



	}

	void updateAnimations(){
		anim.SetBool ("Ground",grounded);
		anim.SetFloat("vSpeed",rigidbody2D.velocity.y);
		anim.SetBool("Crouch",crouching);
		anim.SetFloat("Speed", Mathf.Abs(move));
		anim.SetBool("Dashing",dashing);
		anim.SetBool("Sneak",sneaking);
		anim.SetBool ("DoubleJump",doubleJump);
		anim.SetBool ("Sliding",sliding);
		anim.SetBool ("WallSlide",wallSliding);
		anim.SetBool ("Climbing",climbing);
		anim.SetBool ("OnLadder",ladder);
		anim.SetBool ("Hanging",wallHanging);
	}

	void adjustCollisionBox(){
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
	
	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	public void OnTriggerStay2D(Collider2D col)
	{

		if(col.gameObject.tag == "Ladder" && !sliding)
		{
			ladder = true;
			crouching = false;
			rigidbody2D.gravityScale = 0f;
		}
	}

	public void OnTriggerExit2D(Collider2D col)
	{
		
		if(col.gameObject.tag == "Ladder")
		{
			ladder = false;
			climbing = false;
			rigidbody2D.gravityScale = 1f;
		}
	}
	private void InputColorChange(){
		if(Input.GetKeyDown(KeyCode.R)){
			ChangeColorTo(new Vector3(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)));
		}
		
		
	}
	//-----------------------------------------------------------------------------------------------------------------------------------------
	private void SyncedMovement(){

		if(otherMove > 0 && !facingRight){
			flip();
		} else if(otherMove < 0 && facingRight){
			flip();
		}

		anim.SetFloat("Speed", Mathf.Abs(otherMove));
		anim.SetBool ("Ground",otherGrounded);
		anim.SetFloat("vSpeed",otherVSpeed);
		anim.SetBool("Crouch",otherCrouch);
		anim.SetBool("Dashing",otherDash);
		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(syncStartPosition,syncEndPosition,syncTime/syncDelay);
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {

        if (stream.isWriting){
            stream.SendNext(transform.position);
			stream.SendNext(rigidbody2D.velocity);
			stream.SendNext(move);
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

	//-----------------------------------------------------------------------------------------------------------------------------------------
	[RPC] void ChangeColorTo(Vector3 color){
		renderer.material.color = new Color(color.x,color.y,color.z,1f);
		if(photonView.isMine){
			photonView.RPC ("ChangeColorTo",PhotonTargets.OthersBuffered,color);
		}
	}

	[RPC] void displayText(string str){
		if(str[0] != '/'){
			displayTheText = true;
			textToDisplay = str;
			displayTextCount = 0f;
			if(photonView.isMine){
				photonView.RPC ("displayText",PhotonTargets.OthersBuffered,str);
			}
		}
	}
}
