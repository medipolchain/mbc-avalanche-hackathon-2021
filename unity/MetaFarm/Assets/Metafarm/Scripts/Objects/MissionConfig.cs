using UnityEngine;
using System.Collections;

[System.Serializable]
public class MissionConfig {

	//Available missions which can easily get extended.
	//You as the developer can add up to 4 mission for each level, and you can choose your missions from these available
	//premade missions.

	//Important. Meat is not available as a mission in this version. We will add meat support in the next update.

	public enum missions { MoneyRequired, EggsRequired, BreadRequired, CakeRequired, MilkRequired, MeatRequired, ChickenRequired, CowRequired }
	public missions missionType;
	public int requiredAmount;

}
