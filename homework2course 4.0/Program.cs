using System;
using System.Collections.Generic;
using System.Threading;
public class EventData
{
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}

public delegate void EventHandler(object sender, EventData eventData);

public class EventBus
{
    private Dictionary<string, List<EventHandler>> eventHandlers = new Dictionary<string, List<EventHandler>>();
    private Dictionary<string, DateTime> lastEventTimes = new Dictionary<string, DateTime>();
    private int throttleMilliseconds;

    public EventBus(int throttleMilliseconds = 0)
    {
        this.throttleMilliseconds = throttleMilliseconds;
    }

    public void Register(string eventName, EventHandler eventHandler)
    {
        if (!eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName] = new List<EventHandler>();
        }
        eventHandlers[eventName].Add(eventHandler);
    }

    public void Unregister(string eventName, EventHandler eventHandler)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            eventHandlers[eventName].Remove(eventHandler);
        }
    }

    public void Publish(string eventName, EventData eventData)
    {
        if (eventHandlers.ContainsKey(eventName))
        {
            var currentTime = DateTime.UtcNow;
            if (throttleMilliseconds > 0)
            {
                if (lastEventTimes.ContainsKey(eventName) && (currentTime - lastEventTimes[eventName]).TotalMilliseconds < throttleMilliseconds)
                {
                    return; 
                }
                lastEventTimes[eventName] = currentTime;
            }

            foreach (var handler in eventHandlers[eventName])
            {
                handler(this, eventData);
            }
        }
    }
}


class Program
{
    static void Main(string[] args)
    {
        var eventBus = new EventBus(throttleMilliseconds: 1000); 
        eventBus.Register("event1", Event1Handler);
        eventBus.Register("event2", Event2Handler);

        var eventData1 = new EventData { Message = "Event 1 occurred", Timestamp = DateTime.UtcNow };
        var eventData2 = new EventData { Message = "Event 2 occurred", Timestamp = DateTime.UtcNow };

        eventBus.Publish("event1", eventData1);
        Thread.Sleep(500);
        eventBus.Publish("event2", eventData2);
        Thread.Sleep(500);
        eventBus.Publish("event1", eventData1);
        Thread.Sleep(500);
        eventBus.Publish("event2", eventData2);

        Console.ReadLine();
    }

    private static void Event2Handler(object sender, EventData eventData)
    {
        throw new NotImplementedException();
    }

    private static void Event1Handler(object sender, EventData eventData)
    {
        throw new NotImplementedException();
    }
}


