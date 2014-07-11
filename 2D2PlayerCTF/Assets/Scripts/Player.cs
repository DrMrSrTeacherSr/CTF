using UnityEngine;
using System.Collections;

public class Player : Photon.MonoBehaviour {

	float move;
	private bool facingRight = true;
	Animator anim;
	
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

	//Syncing
	//----------------------------------------------------------------------
	private float otherMove;
	private float otherVSpeed;
	private bool otherGrounded; //Condense into 0101010101
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
		if((grounded||!doubleJump) && Input.GetKeyDown(KeyCode.W) && !sliding){ 
			Vector3 v = rigidbody2D.velocity;
				v.y = 12.5f;
			rigidbody2D.velocity = v;
			grounded = false;
			if(!doubleJump && !grounded)
				doubleJump = true;
		} 
	}

	void checkForCrouch(){
		if(grounded){
			if(grounded && Input.GetKey(KeyCode.S)){
				if(dashing && grounded){
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
				crouching = false;
			}
			if(sliding && (slideCounter >= slideTimer/3 && (Input.GetKeyUp(KeyCode.S)) || 
		             (slideCounter >= slideTimer))){
				if(slideCounter >= 3*slideTimer/4){
					crouching = true;
				} else if(slideCounter >= 3*slideTimer/8){

				}else if(slideCounter >= slideTimer/3){
					dashing = true;
				}
				sliding = false;

			} else if (sliding && slideCounter < slideTimer){
				slideCounter += 1*Time.deltaTime;
			}
		}else{
			crouching = false;
			sliding = false;
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
		getMaxSpeed();

		grounded = Physics2D.OverlapCircle(groundCheck.position, goundRadius, whatIsGround);
		if(grounded)
			doubleJump = false;

		if(move > 0 && !facingRight){
			flip();
		} else if(move < 0 && facingRight){
			flip();
		}

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		updateAnimations();
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

	}
	
	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
