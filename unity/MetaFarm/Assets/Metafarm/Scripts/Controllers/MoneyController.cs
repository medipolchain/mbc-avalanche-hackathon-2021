using UnityEngine;
using System.Collections;

public class MoneyController : MonoBehaviour {

	void Start () {

		//show player money in the UI
		GetComponent<TextMesh>().text = "" + PlayerPrefs.GetInt("PlayerMoney");
	}
}
