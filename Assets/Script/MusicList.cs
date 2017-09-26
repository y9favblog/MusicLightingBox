using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MusicListBase{
	[Tooltip("再生する音源データ")]
	public AudioClip music;
	[Tooltip("再生ボリューム")]
	public float vol;
	[Tooltip("ライティングを変化させる周期"),Range(0.15f,1.0f)]
	public float between;
	[Tooltip("ライティングを変化させる閾値 〜1024、〜2048、〜4096Hzの3つを設定")]
	public int[] thr;
	[Tooltip("音ゲーのノーツの閾値(音ゲーモードを使用しない場合は不要)")]
	public int n_thr;
}

[CreateAssetMenu]
public class MusicList : ScriptableObject{
	public MusicListBase[] m_list;
}
