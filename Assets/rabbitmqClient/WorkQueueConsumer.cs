using UnityEngine;
using System.Collections;
using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

using UnityEngine.UI;
using RabbitMQ.Client.Exceptions;


public class WorkQueueConsumer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	public void changeQueue()
	{
		Text text;
		text =  GameObject.Find("queueText").GetComponent<Text>();
		text.text = "Work Queue";
	}

	public void  Worker()
	{

		Text log = GameObject.Find("console").GetComponent<Text>();

		var factory = new ConnectionFactory() { HostName = "diablo" ,UserName = "guest" ,Password = "guest"};
		using(var connection = factory.CreateConnection())
			using(var channel = connection.CreateModel())
		{

			channel.BasicQos(0, 1, false); //basic quality of service
			QueueingBasicConsumer consumer = new QueueingBasicConsumer(channel);
			channel.BasicConsume("Work_Queue", null, consumer);
			while (true)
			{

				try 
				{
					RabbitMQ.Client.Events.BasicDeliverEventArgs e =
						(RabbitMQ.Client.Events.BasicDeliverEventArgs)
							consumer.Queue.Dequeue();
					IBasicProperties props = e.BasicProperties;
					byte[] body = e.Body;
					// ... process the message
					channel.BasicAck(e.DeliveryTag, false);
				} catch (OperationInterruptedException ex) 
					{
					Debug.Log(ex);
					//break;
					}

				}
		}
	}
}
