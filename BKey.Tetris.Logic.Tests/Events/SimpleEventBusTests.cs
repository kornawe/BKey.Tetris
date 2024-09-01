using System;
using BKey.Tetris.Logic.Events;
using Xunit;

namespace BKey.Tetris.Logic.Tests.Events;

public class SimpleEventBusTests
{
    private readonly SimpleEventBus _eventBus;

    public SimpleEventBusTests()
    {
        _eventBus = new SimpleEventBus();
    }

    [Fact]
    public void Subscribe_ShouldAddHandler()
    {
        // Arrange
        var wasCalled = false;
        void Handler(string message) => wasCalled = true;

        // Act
        _eventBus.Subscribe<string>(Handler);
        _eventBus.Publish("Test");

        // Assert
        Assert.True(wasCalled);
    }

    [Fact]
    public void Unsubscribe_ShouldRemoveHandler()
    {
        // Arrange
        var wasCalled = false;
        void Handler(string message) => wasCalled = true;

        _eventBus.Subscribe<string>(Handler);

        // Act
        _eventBus.Unsubscribe<string>(Handler);
        _eventBus.Publish("Test");

        // Assert
        Assert.False(wasCalled);
    }

    private class TestEventA
    {

    }

    private class TestEventB
    {

    }

    [Fact]
    public void Publish_ShouldNotCallHandlerForDifferentType()
    {
        // Arrange
        var callCount = 0;
        void Handler(TestEventA asdf) => callCount++;

        _eventBus.Subscribe<TestEventA>(Handler);

        // Act
        _eventBus.Publish(new TestEventB()); // Different type
        _eventBus.Publish(new TestEventA()); // Correct type

        // Assert
        Assert.Equal(1, callCount);
    }

    [Fact]
    public void Subscribe_ShouldThrowArgumentNullException_WhenHandlerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _eventBus.Subscribe<string>(null!));
    }

    [Fact]
    public void Unsubscribe_ShouldThrowArgumentNullException_WhenHandlerIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _eventBus.Unsubscribe<string>(null!));
    }

    [Fact]
    public void Publish_ShouldThrowArgumentNullException_WhenEventMessageIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _eventBus.Publish<string>(null!));
    }

    [Fact]
    public void Publish_ShouldInvokeAllSubscribedHandlers()
    {
        // Arrange
        var callCount = 0;
        void Handler1(string message) => callCount++;
        void Handler2(string message) => callCount++;

        _eventBus.Subscribe<string>(Handler1);
        _eventBus.Subscribe<string>(Handler2);

        // Act
        _eventBus.Publish("Test");

        // Assert
        Assert.Equal(2, callCount);
    }
}