using UnityEngine;
using System.Collections;
using System;


using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;
using System.Text;
using UnityEngine.UI;


public class Publish_subscribe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("START");
		/*	var factory = new ConnectionFactory() { HostName = "diablo" };
			using(var connection = factory.CreateConnection())
				using(var channel = connection.CreateModel())
			{
				channel.ExchangeDeclare(exchange: "publicSubscribe", type: "fanout");
				
			var message ="Mensagem";
				

				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish(exchange: "publicSubscribe", routingKey: "", basicProperties: null, body: body);
				Debug.Log(" [x] Sent {0}" + message);
			}*/
			
	}

	// Update is called once per frame
	void Update () {
		
	}
	public void Teste(){
		Debug.Log("Funfo");
	}
	public void changeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Publish / Subscribe Queue";
	}
}
