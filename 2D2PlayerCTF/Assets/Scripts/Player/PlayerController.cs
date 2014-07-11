using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour {

	public PlayerModel model;
	public PlayerView view;

	public float crawlSpeed = 1f;
	public float walkSpeed = 2f;
	public float runSpeed = 6f;
	
	public Transform groundCheck;
	float goundRadius = 0.1f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;

	public float dashTimer = 0.5f;
	private float dashCounter = 0f;
	
	void FixedUpdate () {
		if(photonView.isMine){
			InputMovement();
			InputColorChange();
		} else {
			//SyncedMovement();
		}
	}
	
	void Update(){
		
		if(Input.GetKeyUp(KeyCode.T)){
			view.cheatText = true;
		}
		
		if(model.dashing && (Input.GetKeyUp(KeyCode.D)||Input.GetKeyUp(KeyCode.A)) && dashTimer < 0 ){
			model.dashing = false;
		}
		
		if(Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.A) && model.grounded && !model.crouching){
			if ( dashTimer > 0 && dashCounter == 1/*Number of Taps you want Minus One*/){
				model.dashing = true;
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
		
		
		if(model.grounded && Input.GetAxis("Jump") > 0.01){ 
			Vector3 v = rigidbody2D.velocity;
			if(model.dashing)
				v.y = 15f;
			else
				v.y = 10f;
			rigidbody2D.velocity = v;
		} 
		
		if(model.grounded && Input.GetAxis("Jump") < 0){
			model.crouching = true;
			model.dashing = false;
		} else {
			model.crouching = false;
		}
		
		
		
		
	}
	
	private void InputColorChange(){
		if(Input.GetKeyDown(KeyCode.R)){
			//ChangeColorTo(new Vector3(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f)));
		}
		
		
	}
	
	void InputMovement(){
		
		if(model.dashing){
			model.maxSpeed = runSpeed;
		} else
			model.maxSpeed = walkSpeed;
		if(model.crouching){
			model.maxSpeed = crawlSpeed;
		}
		
		model.moving = Input.GetAxis("Horizontal");
		model.grounded = Physics2D.OverlapCircle(groundCheck.position, goundRadius, whatIsGround);

		
		rigidbody2D.velocity = new Vector2(model.moving * model.maxSpeed, rigidbody2D.velocity.y);
		Debug.Log(rigidbody2D.velocity );
		
		if(model.moving > 0 && !model.facingRight){
			flip();
		} else if(model.moving < 0 &&  model.facingRight){
			flip();
		}
		
	}

	void flip(){
		model.facingRight = !model.facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
