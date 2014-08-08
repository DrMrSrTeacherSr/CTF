using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float length;
	private float counter = 0f;

	public Timer(float length){
		this.length = length;
	}

	public void start(){
		counter = 0f;
	}

	public void tick(){
		counter += Time.deltaTime;
	}

	public void setLength(float newLength){
		length = newLength;
	}
	
	public float getLength(){
		return length;
	}

	public float getCurrentTime(){
		return counter;
	}

	public float getPercentDone(){
		return counter/length;
	}
	public bool isDone(){
		return counter >= length;
	}




}
