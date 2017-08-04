using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

	private Text _fpsText;

	public void Start()
	{
		_fpsText = GetComponent<Text>();
	}
	void Update () {
		_fpsText.text = "" + (int)(1f / Time.deltaTime);
	}
}
