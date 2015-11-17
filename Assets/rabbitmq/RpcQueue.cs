using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;
using System.Text;

public class RpcQueue : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ServerRPCQueue(){
				
		var factory = new ConnectionFactory() { HostName = "diablo" , UserName = "guest", Password = "guest"};
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			//channel.QueueDeclare("rpc_queue"); //version shup
			channel.QueueDeclare("rpc_queue",true,false,false,null);
			channel.BasicQos(0, 1, false);
			var consumer = new QueueingBasicConsumer(channel);

			//channel.BasicConsume("rpc_queue",null,consumer); //version shup
			channel.BasicConsume("rpc_queue",true,consumer);

			//Text log = GameObject.Find("console").GetComponent<Text>();
			//log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Aguardando Requisições RPC";
			
			while(true)
			{	Text log = GameObject.Find("console").GetComponent<Text>();
				log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Aguardando Requisições RPC";

				byte[] response = null;
				var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
				
				var body = ea.Body;
				var props = ea.BasicProperties;
				var replyProps = channel.CreateBasicProperties();
				replyProps.CorrelationId = props.CorrelationId;
				
				try
				{
					Utils.SaveFileToDisk("rpcRecebido.png",body);
					//var fileInfo = new System.IO.FileInfo("rabbitVideo.ogg");
					//usar tamanho arquivo para validacao
					//var fileSize = (fileInfo.Length/1024f)/1024f;
					string filepath = Utils.GetFullPathFileName("rpc.png");
					response = Utils.GetFileAsBytesOrNull (filepath);
				}
				catch(Exception e)
				{
					log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Error: "  + e.Message;
					response = null;
				}
				finally
				{
					var responseBytes = response;
					channel.BasicPublish(exchange: "",
					                     routingKey: props.ReplyTo,
					                     basicProperties: replyProps,
					                     body: responseBytes);
					channel.BasicAck(deliveryTag: ea.DeliveryTag,
					                 multiple: false);
				}
			}
		}
	}
	
	public void AtualizaRecebidas(String message){
		Text text ,log;
		text =  GameObject.Find("TextPR").GetComponent<Text>();
		int count = int.Parse(text.text) + 1;
		text.text= count.ToString();
		log = GameObject.Find("console").GetComponent<Text>();
		log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Mensagem Recebida RPC Queue : " + message + "\n";
		
	}
	public void AtualizaEnviadas(String message){
		Text log ,pe;
		pe = GameObject.Find("TextPE").GetComponent<Text>();
		int count =int.Parse(pe.text) + 1;
		pe.text= count.ToString();
		log = GameObject.Find("console").GetComponent<Text>();
		log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Mensagem Recebida RPC Queue : " + message + "\n";
		
	}
}
