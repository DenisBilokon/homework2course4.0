using System;
using System.Collections.Generic;

public class Event
{
    public string Name { get; set; }
    public int Priority { get; set; }

    public Event(string name, int priority)
    {
        Name = name;
        Priority = priority;
    }
}

public interface ISubscriber
{
    void HandleEvent(Event e);
}

public class Publisher
{
    private Dictionary<int, List<ISubscriber>> subscribers = new Dictionary<int, List<ISubscriber>>();

    public void AddSubscriber(ISubscriber subscriber, int priority)
    {
        if (!subscribers.ContainsKey(priority))
        {
            subscribers[priority] = new List<ISubscriber>();
        }
        subscribers[priority].Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber, int priority)
    {
        if (subscribers.ContainsKey(priority))
        {
            subscribers[priority].Remove(subscriber);
        }
    }

    public void PublishEvent(Event e)
    {
        Console.WriteLine($"Publishing event {e.Name} with priority {e.Priority}");

        List<ISubscriber> allSubscribers = new List<ISubscriber>();
        foreach (var priority in subscribers.Keys)
        {
            allSubscribers.AddRange(subscribers[priority]);
        }
        allSubscribers.Sort((a, b) => b.GetHashCode().CompareTo(a.GetHashCode()));

        foreach (var subscriber in allSubscribers)
        {
            subscriber.HandleEvent(e);
        }
    }
}