using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServiceBuilder : MonoBehaviour {

	Dictionary<string,IStateService> allServices = new Dictionary<string,IStateService>();

	public ServiceBuilder(){
		setUpIdle();
		setUpWalking();
		setUpDashing();
		setUpSneaking();
		setUpJumpingDash();
		setUpJumpingWalk();
		setUpCrouching();
		setUpCrawling();
		setUpSliding();
		setUpOnLadder();
		setUpClimbing();
		setUpDoubleJumpingDash();
		setUpDoubleJumpingWalk();
		setUpHanging();
		setUpWallSliding();

	}

	public Dictionary<string,IStateService> getServices(){
		return allServices;
	}

	private void setUpIdle(){
		IStateService idle = new IdleService();
		idle.addNext("walking");
		idle.addNext("dashing");
		idle.addNext("jumpingWalk");
		idle.addNext("sneaking");
		idle.addNext("crouching");
		idle.addNext("onLadder");

		allServices.Add("idle",idle);
	}

	private void setUpWalking(){
		IStateService walking = new WalkingService();
		walking.addNext("idle");
		walking.addNext("dashing");
		walking.addNext("jumpingWalk");
		walking.addNext("sneaking");
		walking.addNext("crawling");
		walking.addNext("onLadder");
			
		allServices.Add("walking",walking);
	}

	private void setUpDashing(){
		IStateService dashing = new DashingService();
		dashing.addNext("jumpingDash");
		dashing.addNext("sliding");

		allServices.Add("dashing",dashing);
	}

	private void setUpSneaking(){
		IStateService sneaking = new SneakService();
		sneaking.addNext("jumpingWalk");
		
		allServices.Add("sneaking",sneaking);
	}

	private void setUpJumpingDash(){
		IStateService jumping = new JumpingDashService();
		jumping.addNext("onLadder");
		jumping.addNext("doubleJumpingDash");
		jumping.addNext("hanging");
		jumping.addNext("wallSliding");

		allServices.Add("jumpingDash",jumping);
	}

	private void setUpJumpingWalk(){
		IStateService jumping = new JumpingWalkService();
		jumping.addNext("onLadder");
		jumping.addNext("doubleJumpingWalk");
		jumping.addNext("hanging");
		jumping.addNext("wallSliding");

		allServices.Add("jumpingWalk",jumping);
	}
	
	private void setUpCrouching(){
		IStateService crouching = new CrouchingService();
		crouching.addNext("crawling");

		allServices.Add("crouching",crouching);
	}

	private void setUpCrawling(){
		IStateService crawling = new CrawlingService();
		
		allServices.Add("crawling",crawling);
	}

	private void setUpSliding(){
		IStateService sliding = new SlidingService();
		
		allServices.Add("sliding",sliding);
	}

	private void setUpOnLadder(){
		IStateService onLadder = new OnLadderService();
		onLadder.addNext("walking");
		onLadder.addNext("jumpingWalk");
		onLadder.addNext("climbing");

		allServices.Add("onLadder",onLadder);
	}

	private void setUpClimbing(){
		IStateService climbing = new ClimbingService();
		climbing.addNext("jumpingWalk");
		climbing.addNext("climbing");
		
		allServices.Add("climbing",climbing);
	}

	private void setUpDoubleJumpingDash(){
		IStateService doubleJumping = new DoubleJumpingDashService();
		doubleJumping.addNext("onLadder");
		doubleJumping.addNext("wallSliding");
		
		allServices.Add("doubleJumpingDash",doubleJumping);
	}
	
	private void setUpDoubleJumpingWalk(){
		IStateService doubleJumping = new DoubleJumpingWalkService();
		doubleJumping.addNext("onLadder");
		doubleJumping.addNext("wallSliding");
		
		allServices.Add("doubleJumpingWalk",doubleJumping);
	}

	private void setUpHanging(){
		IStateService hanging = new HangingService();
		//hanging.addNext("hanging");
		
		allServices.Add("hanging",hanging);
	}

	private void setUpWallSliding(){
		IStateService wallSliding = new WallSlidingService();
		wallSliding.addNext("hanging");
		
		allServices.Add("wallSliding",wallSliding);
	}

}
