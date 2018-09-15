/*
 * This script control almost the game: State, UI, Ads
*/
using UnityEngine;
using System.Collections;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif
public class GameManager : MonoBehaviour {
	public static GameManager instance;
	public enum GameState{
		Menu,
		Playing,
		Pause,
		Dead
	};

	private GameState state;
	private int score = 0;

	public static GameState CurrentState{
		get{ return instance.state; }
		set{ instance.state = value; }
	}

	public static int Score{
		get{ return instance.score; }
		set{ instance.score = value; }
	}

	public static int HighLevel{
		get{ return PlayerPrefs.GetInt(GlobalValue.levelHighest,1); }
		set{ PlayerPrefs.SetInt(GlobalValue.levelHighest, value); }
	}

	// Use this for initialization
	void Start () {
		instance = this;
		state = GameState.Menu;

	}

	public void Play(){
		state = GameState.Playing;
		GlobalValue.levelPlayingPathLeft = GlobalValue.levelPlaying;	//for PlayerController
		GlobalValue.levelPathLeft = GlobalValue.levelPlaying;		//for CreatePlatform

//		AdsController.HideAds ();

	}

	public void GameSuccess(){
		GlobalValue.levelPlaying++;

		if (GlobalValue.levelPlaying >= HighLevel)
			HighLevel++;		//save playerPref
		
		StartCoroutine (WaitForRestart (1.5f));
	
//		AdsController.ShowAds ();
	
	}

	public void GameOver(){
		state = GameState.Dead;
		StartCoroutine (WaitForRestart (1f));

//		AdsController.ShowAds ();
	
	}

	public void Restart(){
		StartCoroutine (WaitForRestart (0f));
	}

	IEnumerator WaitForRestart(float time){
		yield return new WaitForSeconds (time);
		GlobalValue.isRestart = true;

		#if UNITY_5_3
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		#else
		Application.LoadLevel (Application.loadedLevel);
		#endif
	}
}
