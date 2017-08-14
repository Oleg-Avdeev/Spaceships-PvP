using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class SpawnCardController : MonoBehaviour {
	private Image _image;
	private double _endTime = 0, _maxTime = 0;

	public void SetTimer(double time, double maxTime)
	{
		if(_image == null)
		{
			_image = GetComponent<Image>();
		}
		StopAllCoroutines();

		_maxTime = maxTime;
		_endTime = Time.time + time;
		StartCoroutine("FillTimer");
	}

	IEnumerator FillTimer()
	{
		while(Time.time < _endTime)
		{
			_image.fillAmount = (1f - (float)((_endTime - Time.time) / _maxTime));
			yield return new WaitForSeconds(0.04f);
		}
		_image.fillAmount = 1;
	}
}
