using UnityEngine;
using System.Collections;

public class AtlasTextureAnimator : MonoBehaviour {

	/// <summary>
	/// This class animates an atlas texture by looping through all it's devided frames.
	/// </summary>

	public bool stopAnimation = false;

	public int tileX = 3;
	public int tileY = 3;
	public int animSpeed = 16;

	private int index;
	private Vector2 size;
	private int uIndex;
	private int vIndex;
	private Vector2 offset;

	internal float startTime;
	private bool killFlag;

	void Awake() {
		index = 0;
		startTime = Time.time;
		killFlag = false;
	}
		
	//this function will be called from PetManager class.
	public void kill() {
		startTime = Time.time;
		killFlag = true;
	}

	public void reset() {
		startTime = Time.time;
	}

	
	void Update() {
		index = (int)(( (Time.time - startTime) * animSpeed) % (tileX * tileY));
		size = new Vector2(1.0f / tileX, 1.0f / tileY);
		uIndex = index % tileX;
		vIndex = index / tileX;
		offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
		GetComponent<Renderer>().material.SetTextureScale ("_MainTex", size);
		
		if(stopAnimation)
			GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", new Vector2(0,0) );

		if(index == (tileX * tileY) - 1 && killFlag) {
			print (gameObject.name + " has died.");
			Destroy (gameObject);
		}
	}
}
