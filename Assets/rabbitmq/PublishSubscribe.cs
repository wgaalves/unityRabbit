using UnityEngine;
using System.Collections;
using System;


using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;
using System.Text;
using UnityEngine.UI;


public class PublishSubscribe : MonoBehaviour {

	// Use this for initialization
	void Start () {

			
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void changeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Publish / Subscribe Queue";
	}

	public void SendPSMessage(){

		string filepath = Utils.GetFullPathFileName("Chegou.png");
		byte[] messageBytes = Utils.GetFileAsBytesOrNull (filepath);

		var factory = new ConnectionFactory() { HostName = "diablo" , UserName = "guest" , Password = "guest"};
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			channel.ExchangeDeclare(exchange: "publishSubEX", type: "fanout");
			
			var body = messageBytes;
			//var body = Encoding.UTF8.GetBytes(message);
			channel.BasicPublish(exchange: "publishSubEX",
			                     routingKey: "",
			                     basicProperties: null,
			                     body: body);

			Text text ,log;
			text =  GameObject.Find("TextPE").GetComponent<Text>();
			int count = int.Parse(text.text) + 1;
			text.text= count.ToString();
			log = GameObject.Find("console").GetComponent<Text>();
			var fileInfo = new System.IO.FileInfo("Chegou.png");
			var fileSize = (fileInfo.Length/1024f)/1024f;
			log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Mensagem Enviada Publish / Subscribe : " + fileSize.ToString("0.00") + " MB" + "\n";

		}
	
	}
	public void receivePSMessage(){
		var factory = new ConnectionFactory() { HostName = "diablo", UserName = "guest", Password = "guest"  };
        using(var connection = factory.CreateConnection())
        using(var channel = connection.CreateModel())
        {
		channel.ExchangeDeclare(exchange: "publishSubEX", type: "fanout");
			
			var queueName = channel.QueueDeclare();
			channel.QueueBind(queueName,"publishSubEX","",true,null);
			Text log = GameObject.Find("console").GetComponent<Text>();
			log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Aguardando mensagens.\n";
			
			var consumer = new EventingBasicConsumer();
			consumer.Received += (model, ea) =>
			{
				/*var body = ea.Body;
				Utils.SaveFileToDisk("rabbit.png",body);
				var fileInfo = new System.IO.FileInfo("rabbit.png");
				var fileSize = (fileInfo.Length/1024f)/1024f;
				log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Tamanho mensagem recebida."+ fileSize +" MB\n";*/
				
			};
			channel.BasicConsume(queueName, null,consumer);

		}
	}
}
