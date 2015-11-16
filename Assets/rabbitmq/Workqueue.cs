using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RabbitMQ.Client;
using System.Text;
using System;

public class Workqueue : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void changeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Work Queue";
	}

	 public void NewTask(){
		string filepath = Utils.GetFullPathFileName("Chegou.png");
		byte[] body = Utils.GetFileAsBytesOrNull (filepath);
		var factory = new ConnectionFactory() { HostName = "diablo" };
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			channel.QueueDeclare("Work_Queue");

			var properties = channel.CreateBasicProperties();
			properties.SetPersistent(true);
			
			channel.BasicPublish(exchange: "",
			                     routingKey: "Work_Queue",
			                     basicProperties: properties,
			                     body: body);
			Text text ,log;
			text =  GameObject.Find("TextPE").GetComponent<Text>();
			int count = int.Parse(text.text) + 1;
			text.text= count.ToString();
			log = GameObject.Find("console").GetComponent<Text>();
			var fileInfo = new System.IO.FileInfo("Chegou.png");
			var fileSize = (fileInfo.Length/1024f)/1024f;
			log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Mensagem Enviada Work Queue : " + fileSize.ToString("0.00") + " MB" + "\n";
			
			connection.Close();
			//Console.WriteLine(" [x] Sent {0}", message);
		}

	}
}
