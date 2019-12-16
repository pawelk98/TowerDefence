using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class openIntro : MonoBehaviour {

	public bool jumpToIntroIfNeverOpened;
	private bool introSceneOpened = false;
	private static openIntro instance = null;

	// Use this for initialization
	void Start () {
		if (instance == null) {
			instance = this;
		} else {
			if(instance != this)
				Destroy (this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (SceneManager.GetActiveScene ().name == "Scenes/intro") {
			introSceneOpened = true;
		}


		if (!introSceneOpened && jumpToIntroIfNeverOpened) {
			introSceneOpened = true;
			SceneManager.LoadScene ("Scenes/intro");
		}
	}
}
