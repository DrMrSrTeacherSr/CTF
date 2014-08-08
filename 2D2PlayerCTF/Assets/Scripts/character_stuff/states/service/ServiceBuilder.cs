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

		allServices.Add("idle",idle);
	}

	private void setUpWalking(){
		IStateService walking = new WalkingService();
		walking.addNext("idle");
		walking.addNext("dashing");
		walking.addNext("jumpingWalk");
		walking.addNext("sneaking");
		walking.addNext("crawling");
			
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
		
		allServices.Add("jumpingDash",jumping);
	}

	private void setUpJumpingWalk(){
		IStateService jumping = new JumpingWalkService();
		
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


}
