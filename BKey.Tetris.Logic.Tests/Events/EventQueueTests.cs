using System;
using BKey.Tetris.Logic.Events;
using Moq;
using Xunit;

namespace BKey.Tetris.Logic.Tests.Events;

public class EventQueueTests
{

    [Fact]
    public void Constructor_ShouldSubscribeToEventBus()
    {
        // Arrange & Act
        var mockEventBus = new Mock<IEventBus>();
        using var eventQueue = new EventQueue<string>(mockEventBus.Object);

        // Assert
        mockEventBus.Verify(bus => bus.Subscribe<string>(It.IsAny<Action<string>>()), Times.Once);
    }

    [Fact]
    public void HandleEvent_ShouldEnqueueItem()
    {
        // Arrange
        var eventMessage = "TestEvent";
        var eventBus = new SimpleEventBus();
        using var eventQueue = new EventQueue<string>(eventBus);

        // Act
        eventBus.Publish(eventMessage);

        // Assert
        Assert.Equal(1, eventQueue.Count);
        Assert.Equal(eventMessage, eventQueue.Dequeue());
    }

    [Fact]
    public void Dequeue_ShouldReturnItemInFIFOOrder()
    {
        // Arrange
        var eventMessage1 = "TestEvent1";
        var eventMessage2 = "TestEvent2";
        var eventBus = new SimpleEventBus();
        using var eventQueue = new EventQueue<string>(eventBus);

        // Act
        eventBus.Publish(eventMessage1);
        eventBus.Publish(eventMessage2);

        // Assert
        Assert.Equal(eventMessage1, eventQueue.Dequeue());
        Assert.Equal(eventMessage2, eventQueue.Dequeue());
    }

    [Fact]
    public void DequeueAll_ShouldReturnAllItemsAndClearQueue()
    {
        // Arrange
        var eventMessage1 = "TestEvent1";
        var eventMessage2 = "TestEvent2";
        var eventBus = new SimpleEventBus();
        using var eventQueue = new EventQueue<string>(eventBus);

        eventBus.Publish(eventMessage1);
        eventBus.Publish(eventMessage2);

        // Act
        var allItems = eventQueue.DequeueAll();

        // Assert
        Assert.Equal(2, allItems.Length);
        Assert.Contains(eventMessage1, allItems);
        Assert.Contains(eventMessage2, allItems);
        Assert.Equal(0, eventQueue.Count);
    }

    [Fact]
    public void Count_ShouldReturnNumberOfItemsInQueue()
    {
        // Arrange
        var eventMessage1 = "TestEvent1";
        var eventMessage2 = "TestEvent2";
        var eventBus = new SimpleEventBus();
        using var eventQueue = new EventQueue<string>(eventBus);

        eventBus.Publish(eventMessage1);
        eventBus.Publish(eventMessage2);

        // Act & Assert
        Assert.Equal(2, eventQueue.Count);
    }

    [Fact]
    public void Dispose_ShouldUnsubscribeFromEventBus()
    {
        // Act
        var mockEventBus = new Mock<IEventBus>();
        var eventQueue = new EventQueue<string>(mockEventBus.Object);
        eventQueue.Dispose();

        // Assert
        mockEventBus.Verify(bus => bus.Unsubscribe<string>(It.IsAny<Action<string>>()), Times.Once);
    }

}