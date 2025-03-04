﻿using Azure;
using Azure.Messaging.ServiceBus;

namespace QueueReader;

class Program
{
    static string EndPoint = "zeurbus.servicebus.windows.net";
    static (string Name, string KeY) SasKeyReader = ("lezert", "cEMkeVmsb5lrcM7dPPGFPZ3vnuHTNr41B+ASbPJJddU=");
    static string QueueName = "queuebizz";

    static async Task Main(string[] args)
    {
        //await ReadQueueAsync();
        await ReadQueueProcessorAsync();
        Console.WriteLine("Press Enter to Quit");
        Console.ReadLine();
    }

    private static async Task ReadQueueAsync()
    {
        var cred = new AzureNamedKeyCredential(SasKeyReader.Name, SasKeyReader.KeY);
        var client = new ServiceBusClient(EndPoint, cred);
        var receiver = client.CreateReceiver(QueueName);
        
        do
        {
            var msg = await receiver.ReceiveMessageAsync();
            Console.WriteLine($"Lock Duration: {msg.LockedUntil} Lock Token: {msg.LockToken}");
            var data = msg.Body.ToString();
            Console.WriteLine(data);
            if (data.EndsWith("30"))
            {
                await receiver.AbandonMessageAsync(msg);
                continue;
               // throw new Exception("Ooops");
            }
            await receiver.CompleteMessageAsync(msg);
            //await receiver.AbandonMessageAsync(msg);
            //await receiver.RenewMessageLockAsync(msg);
            await Task.Delay(1000);
        }
        while (true);
    }
    private static async Task ReadQueueProcessorAsync()
    {
        var cred = new AzureNamedKeyCredential(SasKeyReader.Name, SasKeyReader.KeY);
        var client = new ServiceBusClient(EndPoint, cred);
        var receiver = client.CreateProcessor(QueueName, new ServiceBusProcessorOptions { AutoCompleteMessages=false});
        //var receiver = client.CreateSessionProcessor(QueueName);
        
        int i = 0;
        receiver.ProcessMessageAsync += async evtArg => {
           // evtArg.Message.LockedUntil = DateTimeOffset.Now.AddSeconds(20);
            var msg = evtArg.Message;
            Console.WriteLine($"Lock Duration: {msg.LockedUntil} Lock Token: {msg.LockToken} (Session: {msg.SessionId})");
            var data = msg.Body.ToString();
            if (++i % 5 == 0)             
                throw new Exception("Ooops");
            Console.WriteLine(data);
            await evtArg.CompleteMessageAsync(msg);
           
        };

        receiver.ProcessErrorAsync += evtArg => {
            Console.WriteLine("Ook Ooops");
            Console.WriteLine(evtArg.Exception.Message);
            return Task.CompletedTask;
        };

        await receiver.StartProcessingAsync();
        Console.WriteLine("Press Enter to quit processing");
        Console.ReadLine();
        await receiver.StopProcessingAsync();

    }
}
