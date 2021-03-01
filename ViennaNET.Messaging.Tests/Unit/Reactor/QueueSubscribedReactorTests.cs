﻿using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ViennaNET.Messaging.Messages;
using ViennaNET.Messaging.Processing.Impl.Subscribe;
using ViennaNET.Messaging.Tests.Unit.DSL;

namespace ViennaNET.Messaging.Tests.Unit.Reactor
{
  [TestFixture(Category = "Unit", TestOf = typeof(QueueSubscribedReactor))]
  class QueueSubscribedReactorTests
  {
    [Test]
    public void Start_DefaultArgs_NoError()
    {
      var reactor = Given.QueueSubscribedReactor.Please();

      reactor.StartProcessing();

      Assert.Pass();
    }

    [Test]
    public void Stop_DefaultArgs_NoError()
    {
      var reactor = Given.QueueSubscribedReactor.Please();

      reactor.StartProcessing();
      reactor.Stop();

      Assert.Pass();
    }

    [Test]
    public void StartProcessing_WhenSendMessage_ProcessorCalled()
    {
      var message = new TextMessage();
      var processorMock = new Mock<IMessageProcessor>();
      var messageAdapter = Given.MessageAdapter.Please<IMessageAdapterWithSubscribing>();
      var reactor = Given.QueueSubscribedReactor
                         .WithMessageAdapter(messageAdapter)
                         .WithMessageProcessor(processorMock.Object)
                         .Please();

      reactor.StartProcessing();
      messageAdapter.Send(message);

      processorMock.Verify(x => x.Process(message));
    }
  }
}