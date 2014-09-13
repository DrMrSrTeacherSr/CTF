using UnityEngine;
using System.Collections;

public abstract class AbstractItem : MonoBehaviour, IItem {

	private int id;
	private int type;

	public AbstractItem(int id, int type){
		this.id = id;
		this.type = type;
	}

	public int getId(){
		return id;
	}

	public int getType(){
		return type;
	}

	public void useItem(/*probably needs circumstance*/){

	}

}
