using System.IO;
using UnityEngine;
using System.Collections;
using RabbitMQ.Client;
using System.Text;
using System;
using UnityEngine.UI;
using RabbitMQ.Client.Events;


public class SimpleQueueConsumer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ChangeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Single Queue";
	}
	public void ConsumeSimpleQueue(){
	
		var factory = new ConnectionFactory() { HostName = "162.243.220.118" ,UserName = "test" ,Password = "test" };
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			channel.QueueDeclare("SimpleQueue");
			BasicGetResult result = channel.BasicGet("SimpleQueue", true);
			while (result != null)
			{
				string message = Encoding.UTF8.GetString(result.Body);
				result = channel.BasicGet("SimpleQueue", true);
				Atualiza(message);

			}

		}
	}
	public void Atualiza(String message){
		Text text ,log;
		text =  GameObject.Find("TextPR").GetComponent<Text>();
		int count = int.Parse(text.text) + 1;
		text.text= count.ToString();
		log = GameObject.Find("console").GetComponent<Text>();
		log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Mensagem Recebida SingleQueue : " + message + "\n";
	
	}
}