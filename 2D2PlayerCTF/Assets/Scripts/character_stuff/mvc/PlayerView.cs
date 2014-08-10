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

		anim.SetBool("grounded",model.getGrounded());
		anim.SetBool("onLadder",model.getOnLadder());

		switch(model.getCurrentState()){
		
		case "jumping" : anim.SetInteger("animation",-1); break;
		case "idle" : anim.SetInteger("animation",0); break;
		case "walking" : anim.SetInteger("animation",1);break;
		case "dashing" : anim.SetInteger("animation",2);break;
		case "crouching" : anim.SetInteger("animation",3);break;
		case "crawling" : anim.SetInteger("animation",4);break;
		case "sneaking" : anim.SetInteger("animation",5);break;
		case "sliding" : anim.SetInteger("animation",6);break;
		case "onLadder" : anim.SetInteger("animation",7);break;
		case "climbing" : anim.SetInteger("animation",8);break;
		case "doubleJumping" : anim.SetInteger("animation",9);break;




		}


		/*
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
		*/
	}

}
