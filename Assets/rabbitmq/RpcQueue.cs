﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Util;
using System.Text;

public class RpcQueue : MonoBehaviour {

	private IConnection connection;
	private IModel channel;
	private string replyQueueName;
	private QueueingBasicConsumer consumer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void RPConnection(){
		var factory = new ConnectionFactory() {HostName = "diablo" , UserName = "guest", Password = "guest" };
		connection = factory.CreateConnection();
		channel = connection.CreateModel();
		replyQueueName = channel.QueueDeclare().QueueName;
		consumer = new QueueingBasicConsumer(channel);
		channel.BasicConsume(queue: replyQueueName,
		                     noAck: true,
		                     consumer: consumer);
	}
	
	public string Call(string imagem)
	{
		var corrId = Guid.NewGuid().ToString();
		var props = channel.CreateBasicProperties();
		props.ReplyTo = replyQueueName;
		props.CorrelationId = corrId;
		
		string filepath = Utils.GetFullPathFileName(imagem);
		byte[] messageBytes = Utils.GetFileAsBytesOrNull (filepath);
		channel.BasicPublish(exchange: "",
		                     routingKey: "rpc_queue",
		                     basicProperties: props,
		                     body: messageBytes);
		
		while(true)
		{
			var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
			if(ea.BasicProperties.CorrelationId == corrId)
			{
				return Encoding.UTF8.GetString(ea.Body);
			}
		}
	}
	
	public void Close()
	{
		connection.Close();
	}



	public static void ClientRPCQueue()
	{
		var rpcClient = new RpcQueue();
		
		Console.WriteLine(" [x] Requesting fib(30)");
		var response = rpcClient.Call("30");
		Console.WriteLine(" [.] Got '{0}'", response);
		
		rpcClient.Close();
	
	}
	
	public void ServerRPCQueue(){
				
		var factory = new ConnectionFactory() { HostName = "diablo" , UserName = "guest", Password = "guest" };
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{
			channel.QueueDeclare(queue: "rpc_queue",
			                     durable: false,
			                     exclusive: false,
			                     autoDelete: false,
			                     arguments: null);
			channel.BasicQos(0, 1, false);
			var consumer = new QueueingBasicConsumer(channel);
			channel.BasicConsume(queue: "rpc_queue",
			                     noAck: false,
			                     consumer: consumer);
			Text log = GameObject.Find("console").GetComponent<Text>();
			log.text = log.text + "[ "+ DateTime.Now.ToString("HH:mm:ss") +" ] Aguardando Requisições\n";
			
			while(true)
			{
				byte[] response = null;
				var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
				
				var body = ea.Body;
				var props = ea.BasicProperties;
				var replyProps = channel.CreateBasicProperties();
				replyProps.CorrelationId = props.CorrelationId;
				
				try
				{
					//processa requisição
					Utils.SaveFileToDisk("Chegou.png",body);
					AtualizaRecebidas("Chegou.png");
					string filepath = Utils.GetFullPathFileName("rpcRetorno.png");
					response = Utils.GetFileAsBytesOrNull (filepath);
					 
				}
				catch(Exception e)
				{
					log.text = log.text + "Erro : " + e.Message ;
					response = null;
				}
				finally
				{
					var responseBytes =response;
					channel.BasicPublish(exchange: "",
					                     routingKey: props.ReplyTo,
					                     basicProperties: replyProps,
					                     body: responseBytes);
					channel.BasicAck(deliveryTag: ea.DeliveryTag,
					                 multiple: false);
					AtualizaEnviadas("rpcRetorno.png");
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
