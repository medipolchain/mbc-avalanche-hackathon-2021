using UnityEngine;
using System.Collections;

public class SizeAnimator : MonoBehaviour {

	//***************************************************************************//
	// This class will simulate a simple size/scale animation by modifying the objects 
	// scale and position. Used on factories to simulate their production animation.
	//***************************************************************************//

	private float startingWidth;
	private float startingHeight;
	private float startingPosY;
	private float xScaleRatio;
	private float yScaleRatio;
	private bool  animationFlag;
	private float animationSpeed;
	private float animationSpeedBackward;

	IEnumerator Start (){
		//sleep 
		animationFlag = false;
		yield return new WaitForSeconds(0.1f);
		//init
		startingWidth 	= transform.localScale.x;
		startingHeight 	= transform.localScale.y;
		startingPosY = transform.localPosition.y;		
		xScaleRatio = 1.2f;
		yScaleRatio = 1.20f;
		animationSpeed = 3.5f;
		animationSpeedBackward = 1.0f; //Default is 1
		animationFlag = true;
	}

	void Update (){
		if(animationFlag) {
			animationFlag = false;
			StartCoroutine(animateWidth());
			StartCoroutine(animateHeight());
		}
	}

	//****************************
	// Change the scale of the object on Z axis
	//****************************	
	IEnumerator animateHeight (){
		float targetScale = startingHeight * yScaleRatio;
		float tergetPosition;
		tergetPosition = startingPosY - ( ((1 - yScaleRatio) / 2) *  startingHeight);
		
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * animationSpeed;
			transform.localScale = new Vector3(transform.localScale.x,
			                                   Mathf.SmoothStep(startingHeight, targetScale, t),
			                                   transform.localScale.z);
			transform.localPosition = new Vector3(transform.localPosition.x,
			                                      Mathf.SmoothStep(startingPosY, tergetPosition, t),
			                                      transform.localPosition.z);
			yield return 0;
		}
		
		float r = 0.0f; 
		if(transform.localScale.y >= targetScale) {
			while (r <= 1.0f) {
				r += Time.deltaTime * animationSpeed * animationSpeedBackward;
				transform.localScale = new Vector3(transform.localScale.x,
				                                   Mathf.SmoothStep(targetScale, startingHeight, r),
				                                   transform.localScale.z);
				transform.localPosition = new Vector3(transform.localPosition.x,
				                                      Mathf.SmoothStep(tergetPosition, startingPosY, r),
				                                      transform.localPosition.z);
				yield return 0;
			}
		}
		
		/*if(transform.localScale.y <= startingHeight) {
			yield return new WaitForSeconds(0.3f);
			animationFlag = true;
		}*/
	}

	//****************************
	// Change the scale of the object on X axis
	//****************************
	IEnumerator animateWidth (){ 
		yield return new WaitForSeconds(0.15f);
		float targetScale = startingWidth * xScaleRatio;
		float t = 0.0f; 
		while (t <= 1.0f) {
			t += Time.deltaTime * animationSpeed;
			transform.localScale = new Vector3(Mathf.SmoothStep(startingWidth, targetScale, t),
			                                   transform.localScale.y,
			                                   transform.localScale.z);
			yield return 0;
		}
		
		float r = 0.0f; 
		if(transform.localScale.x >= targetScale) {
			while (r <= 1.0f) {
				r += Time.deltaTime * animationSpeed * animationSpeedBackward;
				transform.localScale = new Vector3(Mathf.SmoothStep(targetScale, startingWidth, r),
				                                   transform.localScale.y,
				                                   transform.localScale.z);
				yield return 0;
			}
		}
		
		if(transform.localScale.x <= startingWidth) {
			yield return new WaitForSeconds(0.1f);
			animationFlag = true;
		}
	}

}