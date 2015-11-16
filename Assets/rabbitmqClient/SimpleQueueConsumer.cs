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
	
		var factory = new ConnectionFactory() { HostName = "diablo" ,UserName = "guest" ,Password = "guest" };
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			channel.QueueDeclare("SimpleQueue");
			BasicGetResult result = channel.BasicGet("SimpleQueue", true);
			while (result != null)
			{
				//string message = result.Body;

				Utils.SaveFileToDisk("rabbitVideo.ogg",result.Body);
				result = channel.BasicGet("SimpleQueue", true);
				var fileInfo = new System.IO.FileInfo("rabbitVideo.ogg");
				var fileSize = (fileInfo.Length/1024f)/1024f;
				Atualiza(fileSize.ToString("0.00") + " MB");


			}
			if(result == null){
				Text log;
				log = GameObject.Find("console").GetComponent<Text>();
				log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Não Há mensagens para consumir \n";
			
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