#pragma warning disable 0414

using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdsInitializer : MonoBehaviour
{

	//CHANGE THESE WITH YOUR OWN APP ID IN UNITYADS DASHBOARD
	[SerializeField]
	private string
		androidGameId = 	"1224021",
		iosGameId = 		"1224021";

	//Set the testMode to false for the final build
	[SerializeField]
	private bool enableTestMode = true;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	void Start ()
	{
		string gameId = null;
		
		#if UNITY_ANDROID
		gameId = androidGameId;
		#elif UNITY_IOS
		gameId = iosGameId;
		#endif

		if (string.IsNullOrEmpty(gameId)) { // Make sure the Game ID is set.
			Debug.LogError("Failed to initialize Unity Ads. Game ID is null or empty.");
		} else if (!Advertisement.isSupported) {
			Debug.LogWarning("Unable to initialize Unity Ads. Platform not supported.");
		} else if (Advertisement.isInitialized) {
			Debug.Log("Unity Ads is already initialized.");
		} else {
			Debug.Log(string.Format("Initialize Unity Ads using Game ID {0} with Test Mode {1}.",
			                        gameId, enableTestMode ? "enabled" : "disabled"));
			Advertisement.Initialize(gameId, enableTestMode);
		}
	}
}