using System.IO;
using UnityEngine;
using System.Collections;
using System.Text;
using System;
using UnityEngine.UI;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;


public class RpcQueueConsumer : MonoBehaviour {
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
	public void ChangeQueue(){
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "RPC Queue";
	}

	
	public void SendRpcQueue(){
		
		var factory = new ConnectionFactory() { HostName = "diablo", UserName = "guest" , Password= "guest" };
		connection = factory.CreateConnection();
		channel = connection.CreateModel();
		replyQueueName = channel.QueueDeclare();
		consumer = new QueueingBasicConsumer(channel);
		//channel.BasicConsume(replyQueueName,null,consumer); //version shup
		channel.BasicConsume(replyQueueName,true,consumer);
	}
	
	public string Call(string message)
	{
		var corrId = Guid.NewGuid().ToString();
		var props = channel.CreateBasicProperties();
		props.ReplyTo = replyQueueName;
		props.CorrelationId = corrId;
		
		string filepath = Utils.GetFullPathFileName("rabbit.ogg");
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
	
	
}




