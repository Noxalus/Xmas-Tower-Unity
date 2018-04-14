using UnityEngine;

public class HighscoreBarAnimation : MonoBehaviour {

    public float ScrollX = 0.5f;
    public float ScrollY = 0.5f;

    void Update()
    {
        var material = GetComponentInChildren<Renderer>().material;
        material.mainTextureOffset = new Vector2(Time.time * ScrollX, Time.time * ScrollY);
	}
}
