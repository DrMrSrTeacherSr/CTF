using UnityEngine;
using System.Collections;

public class CrawlingState : AbstractPlayerState {

	private float crawlSpeed = 2f;
	
	
	public CrawlingState(){
		
	}
	
	
	protected override void initializeState(){
		stateName = "crawling";
	}
	
	public override void updateState(PlayerController controller){
		this.controller = controller;
		if(Input.GetKey (KeyCode.A))
			setXVelocity(-crawlSpeed);
		else
			setXVelocity(crawlSpeed);
	}
}
