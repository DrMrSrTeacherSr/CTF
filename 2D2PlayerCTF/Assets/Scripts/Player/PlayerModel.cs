using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerModel : Photon.MonoBehaviour {


	Dictionary<string,int> state;

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

	public void flip(){
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}
