using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenImage : MonoBehaviour {

	[SerializeField] private Texture _bgImage;

	void OnGUI()
	{
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), _bgImage, ScaleMode.StretchToFill);
	}
}
