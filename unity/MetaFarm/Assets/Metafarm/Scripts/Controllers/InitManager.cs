using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InitManager : MonoBehaviour {

	/// <summary>
	/// We are using this class at the start of the game to init master flags of the game.
	/// Then we can safely load the menu scene.
	/// </summary>

	IEnumerator Start () {

		//init the master flags


		//load menu scene
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene("Menu");
	}

}
