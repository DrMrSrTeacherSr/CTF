using UnityEngine;
using System.Collections;

public abstract class AbstractPlayerState : MonoBehaviour, IPlayerState{
	
	protected PlayerController controller;
	protected string stateName;
	protected Timer stateTimer;

	public AbstractPlayerState(){
		initializeState();
	}

	public string getName(){
		return stateName;
	}

	public Timer getTimer(){
		return stateTimer;
	}
	
	protected abstract void initializeState();
	public abstract void updateState(PlayerController controller);

	//Physics stuff
	//--------------------------------------------------------------------------
	protected void setXVelocity(float velX){
		Vector2 velocity = controller.getRigidbody().velocity;
		velocity.x = velX;
		controller.getRigidbody().velocity = velocity;
	}

	protected void setYVelocity(float velY){
		Vector2 velocity = controller.getRigidbody().velocity;
		velocity.y = velY;
		controller.getRigidbody().velocity = velocity;
	}

	protected void setGravity(float gravity){
		controller.getRigidbody().gravityScale = gravity;
	}
	//--------------------------------------------------------------------------

}
