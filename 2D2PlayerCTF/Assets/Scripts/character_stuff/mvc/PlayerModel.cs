using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel : Photon.MonoBehaviour {


	//Dictionary<string,bool> state;
	string currentState;
	float xVelocity = 0f;
	float yVelocity = 0f;
	bool grounded = true;
	bool onLadder = false;
	bool hanging = false;

	void Start(){
		//state = new Dictionary<string, bool>();
		//setUpState();
		currentState = "idle";
	}

	private void setUpState(){
		/*
		state.Add("grounded",false);
		state.Add("crouching",false);
		state.Add("dashing",false);
		state.Add("sneaking",false);
		state.Add("doubleJump",false);
		state.Add("sliding",false);
		state.Add("wallSliding",false);
		state.Add("climbing",false);
		state.Add("onLadder",false);
		state.Add("hanging",false);
		*/
		//state.Add("facingRight",false);
	}
	/*
	public void set(string str,bool boolean){
		state[str] = boolean;
	}

	public bool get(string str){
			return state[str];
	}
	*/

	public void setGrounded(bool ground){
		grounded = ground;
	}

	public void setOnLadder(bool ladder){
		onLadder = ladder;
	}

	public void setHanging(bool hanging){
//		onLadder = ladder;
	}

	public bool getGrounded(){
		return grounded;
	}

	public bool getOnLadder(){
		return onLadder;
	}

	public void setCurrentState(string str){
		currentState = str;
	}

	public string getCurrentState(){
		return currentState;
	}

	public void setPosition(Vector3 pos){
		transform.position = pos;
	}

	public Vector3 getPosition(){
		return transform.position;
	}

	public void setXVelocity(float xV){
		xVelocity = xV;
	}

	public float getXVelocity(){
		return xVelocity;
	}

	public void setYVelocity(float yV){
		yVelocity = yV;
	}

	public float getYVelocity(){
		return yVelocity;
	}


	public void flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}
