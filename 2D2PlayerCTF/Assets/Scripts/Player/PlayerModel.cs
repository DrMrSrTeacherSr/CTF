using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel : Photon.MonoBehaviour {


	Dictionary<string,int> state;
	float xVelocity = 0f;
	float yVelocity = 0f;

	void Start(){
		state = new Dictionary<string, int>();
		setUpState();
	}

	private void setUpState(){
		state.Add("grounded",0);
		state.Add("crouching",0);
		state.Add("dashing",0);
		state.Add("sneaking",0);
		state.Add("doubleJump",0);
		state.Add("sliding",0);
		state.Add("wallSliding",0);
		state.Add("climbing",0);
		state.Add("onLadder",0);
		state.Add("hanging",0);

		state.Add("facingRight",0);
	}

	public void set(string str,bool boolean){
		if(boolean)
			state[str] = 1;
		else
			state[str] = 0;
	}

	public bool get(string str){
		if (state[str] == 0)
			return false;
		else
			return true;
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
