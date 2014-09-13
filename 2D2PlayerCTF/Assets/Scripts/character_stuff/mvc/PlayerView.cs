using UnityEngine;
using System.Collections;

public class PlayerView : Photon.MonoBehaviour {

	PlayerModel model;
	Animator anim;
	ViewGUI gui;

	void Start(){
		model  = GetComponent<PlayerModel>();
		anim = GetComponent<Animator>();
		gui = new ViewGUI(photonView, model);
	}

	void Update(){
		updateAnimations();
	
	}

	void OnGUI(){
			gui.updateGUI();
	}
	
	void updateAnimations(){

		anim.SetFloat("vSpeed",model.getYVelocity());
		anim.SetFloat("Speed",Mathf.Abs(model.getXVelocity()));

		anim.SetBool("grounded",model.getGrounded());
		anim.SetBool("onLadder",model.getOnLadder());

		anim.SetInteger("animation",model.getCurrentState());

	}
	




}
