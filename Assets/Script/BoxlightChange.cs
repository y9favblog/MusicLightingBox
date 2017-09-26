using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxlightChange : MonoBehaviour {
	[SerializeField]AudioSource aus;
	Renderer rend;
	Light l;
	public Color[] l_color;
	public float speed = 0.8f;
	public int c_delay = 0;
	int color_id = 0;
	Vector3 baseScale;
	Vector3[] cScale = new Vector3[3];

	void Awake(){
		baseScale = transform.localScale;

		//ライティングの色に応じてCubeのサイズの変動幅を大きくさせる
		cScale [0] = new Vector3 (baseScale.x / 1.01f, baseScale.y / 1.01f, baseScale.z / 1.01f);
		cScale [1] = new Vector3 (baseScale.x / 1.02f, baseScale.y / 1.02f, baseScale.z / 1.02f);
		cScale [2] = new Vector3 (baseScale.x / 1.04f, baseScale.y / 1.04f, baseScale.z / 1.04f);

		rend = GetComponent<Renderer> ();
		l = GetComponent<Light> ();
		rend.material.EnableKeyword ("_Emisson");	//materialのEmissonパラメータ変更を有効化
	}

	// Use this for initialization
	void Start () {
		StartCoroutine (SizeChange ());
	}


	public void ColorChange(int c){
		rend.material.SetColor("_EmissionColor",l_color[c]);
		l.color = l_color [c];
		color_id = c;
	}

	IEnumerator SizeChange(){
		while(true){
			if (aus.isPlaying) {
				transform.localScale = cScale [color_id];
				yield return new WaitForSeconds (speed / 10);
				transform.localScale = baseScale;
				yield return new WaitForSeconds (speed / 10);
			} else {
				yield return null;
			}
		}
	}
}
