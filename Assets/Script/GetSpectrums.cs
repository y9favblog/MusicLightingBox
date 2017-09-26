using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetSpectrums : MonoBehaviour {
	MusicList m_list;


	public  BoxlightChange[] box;
	private float[] spectrum; //FFTデータ
	AudioSource aus;
	float[] hz = new float[3]; //〜1024、〜2048、〜4096Hz

	public bool MusicGame_mode = false;
	public AudioSource aus_se;

	public bool debug_mode = false;
	int playing_mid;

	float[] thr = {20,6,6}; //デフォルト値

	public Button[] mselect_bt;

	void Awake(){
		m_list = Resources.Load<MusicList>("MusicList");
		for (int i = 0; i < m_list.m_list.Length; i++) {
			mselect_bt [i].interactable = true;
		}
	}

	void Start () {
		aus = GetComponent<AudioSource> ();
		StartCoroutine (GetSpectrum ());
	}
		
	public void MusicChange(int id){
		
		//設定されていないもしくは3つ設定していない場合はデフォルト値を使用
		if (m_list.m_list [id].thr.Length == 3) {
			for (int i = 0; i < 3; i++) {
				thr [i] = m_list.m_list [id].thr [i];
			}
		} else {
			thr = new float[]{ 20, 6, 6 };
		}

		aus.clip = m_list.m_list[id].music;
		aus.volume = m_list.m_list [id].vol;
		playing_mid = id;
		aus.Play ();
	}

	IEnumerator GetSpectrum(){
		while(true){
			if (aus.isPlaying) {
				spectrum = aus.GetSpectrumData (1024, 0, FFTWindow.BlackmanHarris);
				Chack_hz ();
				yield return new WaitForSeconds (m_list.m_list [playing_mid].between);
			} else {
				yield return null;
			}
		}
	}

	void Chack_hz(){
		hz = new float[]{0,0,0};

		for(int i = 0; i < 1024; i++ ) {
			double fq = ((AudioSettings.outputSampleRate / 2) / 1024) * i;
			if (fq < 1024) {
				hz[0] += spectrum [i];
			} else if (fq < 2048) {
				hz[1]+= spectrum [i];
			} else if (fq < 4096) {
				hz[2]+= spectrum [i];
			}
		}
		int tmp = 0;
		for (int i = 0; i < hz.Length; i++) {
			ChangeBoxColor ((int)(hz [i] * 100), i);
			tmp += (int)(hz [i] * 100);
		}

		//問題しかない未完成の音ゲーモード(ノーツの位置にSEがなるだけ)
		if (tmp > m_list.m_list[playing_mid].n_thr && MusicGame_mode) {
			if(debug_mode) Debug.Log ("Sum:" + tmp);
			aus_se.Play ();
		}

	}

	void ChangeBoxColor(int hz,int boxid){
		float vol = aus.volume;

		//周波数帯毎に分類し、それぞれの帯域で鳴っている音量によりCubeのライティングを変化させる
		//hz < x * vol  のxは閾値(適当)、ライティングの点滅が激しかったりしたら変更推奨
		switch (boxid) {
		case 0:
			if (hz < thr[boxid] * vol) {
				box [boxid].ColorChange (0);
			} else if (hz < thr[boxid] * 2 * vol) {
				box [boxid].ColorChange (1);
			} else {
				box [boxid].ColorChange (2);
			}
			break;
		case 1:
			if (hz < thr[boxid] * vol) {
				box [boxid].ColorChange (0);
			} else if (hz < thr[boxid] * 2 * vol) {
				box [boxid].ColorChange (1);
			} else {
				box [boxid].ColorChange (2);
			}
			break;
		case 2:
			if (hz < thr[boxid] * vol) {
				box [boxid].ColorChange (0);
			} else if (hz < thr[boxid] * 2 * vol) {
				box [boxid].ColorChange (1);
			} else {
				box [boxid].ColorChange (2);
			}
			break;
		default:
			break;
		}
	}
}
