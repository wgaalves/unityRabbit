using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RpcQueue : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ChangeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "RPC Queue";
	}
}
