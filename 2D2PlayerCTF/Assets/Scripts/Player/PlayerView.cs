using UnityEngine;
using System.Collections;

public class PlayerView : Photon.MonoBehaviour {

	PlayerModel model;
	Animator anim;

	void Start(){
		model  = GetComponent<PlayerModel>();
		anim = GetComponent<Animator>();
	}

	void Update(){
		updateAnimations();
	}

	void OnGUI(){




	}
	
	void updateAnimations(){

		anim.SetFloat("vSpeed",model.getYVelocity());
		anim.SetFloat("Speed",Mathf.Abs(model.getXVelocity()));

		anim.SetBool ("Ground",model.get("grounded"));
		anim.SetBool("Crouch",model.get("crouching"));
		anim.SetBool("Dashing",model.get("dashing"));
		anim.SetBool("Sneak",model.get("sneaking"));
		anim.SetBool ("DoubleJump",model.get("doubleJump"));
		anim.SetBool ("Sliding",model.get("sliding"));
		anim.SetBool ("WallSlide",model.get("wallSliding"));
		anim.SetBool ("Climbing",model.get("climbing"));
		anim.SetBool ("OnLadder",model.get("onLadder"));
		anim.SetBool ("Hanging",model.get("hanging"));
	}

}
