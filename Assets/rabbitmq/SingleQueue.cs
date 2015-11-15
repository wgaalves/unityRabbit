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
	public void ChangeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Single Queue";
	}

	public void SendSimpleQueue(){
		var factory = new ConnectionFactory() { HostName = "162.243.220.118" ,UserName = "test" ,Password = "test" };
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			channel.QueueDeclare("SimpleQueue");
			
			string message = "Hello World!";
			var body = Encoding.UTF8.GetBytes(message);
			
			channel.BasicPublish(exchange: "",
			                     routingKey: "SimpleQueue",
			                     basicProperties: null,
			                     body: body);

			Text text ,log;
			text =  GameObject.Find("TextPE").GetComponent<Text>();
			int count = int.Parse(text.text) + 1;
			text.text= count.ToString();
			log = GameObject.Find("console").GetComponent<Text>();
			log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Mensagem Enviada SingleQueue : " + message + "\n";
			Debug.Log(" [x] Sent {0}" + message);
			connection.Close();
		}


	}

}


