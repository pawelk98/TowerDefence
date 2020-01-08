using UnityEngine;
using System.Collections;

public class gameMana : MonoBehaviour {

	public int mana;
	public int rerollCost;
	public float time;
	// private bool isCoroutineExecuting = false;
	public GameObject[] crystals = new GameObject[10];


	// Use this for initialization
	void Start () {
		// time = 3f; // ustawia sie w unity editor
		for (int i = 0; i < 10; i++) {
				crystals [i].SetActive (false);
		}
		StartCoroutine(manaChanger());
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < 10; i++)
        {
            if(i < mana) crystals[i].SetActive(true);
            else crystals[i].SetActive(false);
        }
    }

	public IEnumerator manaChanger(){ //Funkcja dodajaca mane cały czas do 10 co time
		
		while(true){
		mana++;
		// Debug.Log(mana.ToString());
		if(mana>=10) mana=10;
		if (mana < 0) mana = 0;
            if (time < 0.01f) time = 0.01f;
		yield return new WaitForSeconds(time);
		}
	}
	

}
