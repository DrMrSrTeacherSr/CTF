using UnityEngine;
using System.Collections;

public class Player : Photon.MonoBehaviour {

	float move;
	public float maxSpeed = 2f;
	public float crawlSpeed = 1f;
	public float walkSpeed = 2f;
	public float runSpeed = 6f;
	private bool facingRight = true;
	public bool dashing = false;
	public float dashTimer = 0.5f;
	private float dashCounter = 0f;

	private float otherMove;
	private float otherVSpeed;
	private bool otherGrounded; //Condense into 0101010101
	private bool otherCrouch;
	private bool otherDash;

	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float goundRadius = 0.1f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;

	public bool crouching = false;

    private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	private string textToDisplay = "";
	private bool displayTheText = false;

	private float displayTextTimer = 10f;
	private float displayTextCount = 0f;

	private bool cheatText = false;
	
	string cheatCode;
	private string cheatField = "";
	public string[] pastComments;

	void Start(){
		anim = GetComponent<Animator>();

	}

	void OnGUI(){
		//GUI.Label(new Rect(10,Screen.height - 60,400,20),"Something");
		if(cheatText){
			GUI.SetNextControlName("MyTextField");
			cheatField = GUI.TextField(new Rect(10,Screen.height - 30,400,20), cheatField, 25);
			if(cheatField != "" && Event.current.keyCode == KeyCode.Return)
			{
				cheatCode = cheatField;
				cheatField = "";
				displayTheText = true;
				photonView.RPC("displayText",PhotonTargets.OthersBuffered,cheatCode);
				cheatText = false;
				displayTextCount = 0f;
			}
			GUI.FocusControl("MyTextField");
		}

		if(displayTheText){
			print(cheatCode);
			GUI.Label(new Rect(10,Screen.height - 60,400,30),cheatCode);
		}
		if( displayTextTimer >= displayTextCount)
			displayTextCount += 1 * Time.deltaTime;
		else
			displayTheText= false;

	} 


	// Update is called once per frame
	void FixedUpdate () {
        if(photonView.isMine){
       		InputMovement();
			InputColorChange();
		} else {
			SyncedMovement();
		}
	}

	void Update(){

		if(Input.GetKeyUp(KeyCode.T)){
			cheatText = true;
		}

		if(dashing && (Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.A)) && dashTimer < 0 ){
			dashing = false;
		}

		if(Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.A) && grounded && !crouching){
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
	
		
		if(grounded && Input.GetAxis("Jump") > 0.01){ 
			Vector3 v = rigidbody2D.velocity;
			if(dashing)
				v.y = 15f;
			else
				v.y = 10f;
			rigidbody2D.velocity = v;
			anim.SetBool("Ground",false);
			
		} 
		
		if(grounded && Input.GetAxis("Jump") < 0){
			crouching = true;
			dashing = false;
		} else {
			crouching = false;
		}




	}

	private void InputColorChange(){
		if(Input.GetKeyDown(KeyCode.R)){
			ChangeColorTo(new Vector3(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)));
		}
		
		
	}

    void InputMovement(){



		if(dashing){
			maxSpeed = runSpeed;
		} else
			maxSpeed = walkSpeed;
		if(crouching){
			maxSpeed = crawlSpeed;
		}

		move = Input.GetAxis("Horizontal");
		grounded = Physics2D.OverlapCircle(groundCheck.position, goundRadius, whatIsGround);

		anim.SetFloat("vSpeed",rigidbody2D.velocity.y);
		anim.SetBool("Crouch",crouching);
		anim.SetFloat("Speed", Mathf.Abs(move));
		anim.SetBool("Dashing",dashing);
		anim.SetBool ("Ground",grounded);

		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		if(move > 0 && !facingRight){
			flip();
		} else if(move < 0 && facingRight){
			flip();
		}

    }
	
	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

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

	
	[RPC] void ChangeColorTo(Vector3 color){
		renderer.material.color = new Color(color.x,color.y,color.z,1f);
		if(photonView.isMine){
			photonView.RPC ("ChangeColorTo",PhotonTargets.OthersBuffered,color);
		}
	}

	[RPC] void displayText(string str){
		displayTheText = true;
		textToDisplay = str;
		GUI.Label(new Rect(0,0,Screen.width,Screen.height),"Something");
	}
}
