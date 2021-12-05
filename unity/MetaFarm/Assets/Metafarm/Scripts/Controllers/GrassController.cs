using UnityEngine;
using System.Collections;

public class GrassController : MonoBehaviour {

	/// <summary>
	/// Main grass controller.
	/// Each grass you plant for your pets, has a certain health that is reduced when a pet eats it.
	/// You can indicate grass health and when it reached to zero, it will automatically destroys itself.
	/// </summary>

	public float health = 500;			
	private int tileX = 4;
	private int tileY = 4;
	private int animSpeed = 16;
	private float startTime;
	private int index;
	private Vector2 size;
	private int uIndex;
	private int vIndex;
	private Vector2 offset;

	//material offsets
	private Vector2[] materialOffset = new Vector2[5];


	void Awake() {
		materialOffset[0] = new Vector2(0,			0.75f);
		materialOffset[1] = new Vector2(0.75f, 		0.75f);
		materialOffset[2] = new Vector2(0.75f, 		0.5f);
		materialOffset[3] = new Vector2(0.75f, 		0.25f);
		materialOffset[4] = new Vector2(0.75f, 		0);
	}

	void Start () {
		startTime = Time.time;
		StartCoroutine(Animate(startTime));
	}	

	void Update () {
		
		if(Time.time > startTime + 1) {
			if(health <= 100 && health >= 80)
				GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", materialOffset[4]);
			else if (health < 80 && health >= 60)
				GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", materialOffset[3]);
			else if (health < 60 && health >= 40)
				GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", materialOffset[2]);
			else if (health < 40 && health >= 20)
				GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", materialOffset[1]);
			else if (health < 20 && health >= 0)
				GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", materialOffset[0]);

		}

		//health -= 1.5f * Time.deltaTime;
		if(health <= 0) 
			Destroy(gameObject);
	}


	//animate grass planting
	IEnumerator Animate (float _time){

		while(index < ((tileX * tileY) - 1) ) {
			index = (int)(( (Time.time - startTime) * animSpeed) % (tileX * tileY));
			size = new Vector2(1.0f / tileX, 1.0f / tileY);
			uIndex = index % tileX;
			vIndex = index / tileX;
			offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);
			GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
			GetComponent<Renderer>().material.SetTextureScale ("_MainTex", size);

			if(index == (tileX * tileY) - 1) {
				yield break;
			}

			yield return 0;
		}
	}
}