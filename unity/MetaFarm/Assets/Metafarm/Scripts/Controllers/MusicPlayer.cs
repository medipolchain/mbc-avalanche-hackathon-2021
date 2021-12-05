using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour {

	//***************************************************************************//
	// Singleton Music Player. It provided continues music play inside all scenes.
	//***************************************************************************//

	static bool globalSoundPlayFlag;
	public AudioClip menuMusic;
	public AudioClip ingameMusic;

	void Awake (){
		globalSoundPlayFlag = true;
		PlayerPrefs.SetInt("soundEnabled", 1);
		//singleton pattern
		DontDestroyOnLoad(gameObject);
	}

	//we can carefully choose the levels we want to play a particular music. for example we want to use menu music 
	//inside menu and selection scenes and the ingame music for the actual game levels.
	void OnLevelWasLoaded ( int level  ){
		if(globalSoundPlayFlag) {
			if(level == 0 || level == 1 || level == 2 || level == 3 || level == 4) {
				GetComponent<AudioSource>().enabled = true;
				GetComponent<AudioSource>().clip = menuMusic;
				if (!GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Play();
			} else {	
				GetComponent<AudioSource>().enabled = true;
				GetComponent<AudioSource>().clip = ingameMusic;
				if (!GetComponent<AudioSource>().isPlaying)
					GetComponent<AudioSource>().Play();
			}
		}		
	}

}