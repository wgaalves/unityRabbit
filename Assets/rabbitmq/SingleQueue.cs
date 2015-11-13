using System.IO;
using UnityEngine;
using System.Collections;
using RabbitMQ.Client;
using System.Text;
using System;
using UnityEngine.UI;

public class SingleQueue : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log(Path.GetFileName("/arquivos/rabbit.ogg"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void changeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Single Queue";
	}
	
}


