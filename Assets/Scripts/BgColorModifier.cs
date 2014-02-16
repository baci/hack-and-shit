using UnityEngine;
using System.Collections;

public class BgColorModifier : MonoBehaviour {
	
	public Color minColor;
	public Color maxColor;

	public AnimationCurve blink;
	public AnimationCurve blinkAcceleration;

	private float _emission;
	public float emission{
		get{return _emission;}
		set{
			_emission = Mathf.Clamp01(value);
			Color c = Color.Lerp(minColor, maxColor, _emission);
			renderer.material.SetColor("_EmisColor", c);
		}
	}

	void Update(){
		float percentage = GameTime.Instance.GetTimePercentage();
		float bSpeed = blink.Evaluate(Time.time%2);
		float b = blinkAcceleration.Evaluate(percentage)*bSpeed;

		emission = b;
	}
}
