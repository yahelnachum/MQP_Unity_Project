using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AugmentedRealityGyro : MonoBehaviour {

	private static AugmentedRealityGyro arGyro = null;

	private GameObject foregroundObj;
	private GameObject backgroundObj;
	private Vector3 initPosForeground;
	private Vector3 initPosBackground;

	const float gyroForegroundMultiplier = 0.15f;
	const float gyroBackgroundMultiplier = 0.075f;
	const float foregroundPullbackMultiplier = 0.01f;
	const float backgroundPullbackMultiplier = 0.001f;

	private AugmentedRealityGyro(){
	}

	public static AugmentedRealityGyro getInstance(){
		if (arGyro == null) {
			GameObject singleton = new GameObject ();
			arGyro = singleton.AddComponent<AugmentedRealityGyro> ();
		}
		return arGyro;
	}

	public void reInitializeForNarrativeChunk(){
		foregroundObj = StartGame.findInactive("pARImageForeground0" + PlayerData.getInstance ().getCurrentNarrativeChunk (),"vMenu")[0];
		backgroundObj = StartGame.findInactive("pARImageBackground","vMenu")[0];

		initPosForeground = foregroundObj.transform.position;
		initPosBackground = backgroundObj.transform.position;
	}

	public void stepGyro(){
		Vector3 input = Input.gyro.rotationRateUnbiased;
		Vector3 inputForeground = new Vector3 (-1f * input.y * gyroForegroundMultiplier, input.x * gyroForegroundMultiplier, input.z);
		Vector3 inputBackground = new Vector3 (-1f * input.y * gyroBackgroundMultiplier, input.x * gyroBackgroundMultiplier, input.z);

		Vector3 curPosForeground = foregroundObj.transform.position;
		Vector3 curPosBackground = backgroundObj.transform.position;

		float gyroXDiffForeground = inputForeground.x + curPosForeground.x;
		float xPullBackForeground = foregroundPullbackMultiplier * (initPosForeground.x - curPosForeground.x);

		float gyroYDiffForeground = inputForeground.y + curPosForeground.y;
		float yPullBackForeground = foregroundPullbackMultiplier * (initPosForeground.y - curPosForeground.y);

		float gyroXDiffBackground = inputBackground.x + curPosBackground.x;
		float xPullBackBackground = backgroundPullbackMultiplier * (initPosBackground.x - curPosBackground.x);

		float gyroYDiffBackground = inputBackground.y + curPosBackground.y;
		float yPullBackBackground = backgroundPullbackMultiplier * (initPosBackground.y - curPosBackground.y);

		foregroundObj.transform.position = new Vector3 (gyroXDiffForeground + xPullBackForeground, 
			gyroYDiffForeground + yPullBackForeground, 
			curPosForeground.z);

		backgroundObj.transform.position = new Vector3 (gyroXDiffBackground + xPullBackBackground, 
			gyroYDiffBackground + yPullBackBackground, 
			curPosBackground.z);
	}

	public void resetGyro(){
		foregroundObj.transform.position = initPosForeground;
		backgroundObj.transform.position = initPosBackground;
	}


}
