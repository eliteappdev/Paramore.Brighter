#region Licence
/* The MIT License (MIT)
Copyright � 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the �Software�), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System;
using System.Threading.Tasks;
using NUnit.Framework;
using paramore.brighter.commandprocessor.tests.nunit.CommandProcessors.TestDoubles;
using paramore.brighter.commandprocessor.tests.nunit.MessageDispatch.TestDoubles;
using paramore.brighter.serviceactivator;
using paramore.brighter.serviceactivator.TestHelpers;

namespace paramore.brighter.commandprocessor.tests.nunit.MessageDispatch
{
    [TestFixture]
    public class MessagePumpUnacceptableMessageLimitBreachedTests
    {
        private IAmAMessagePump _messagePump;
        private FakeChannel _channel;
        private SpyRequeueCommandProcessor _commandProcessor;

        [SetUp]
        public void Establish()
        {
            _commandProcessor = new SpyRequeueCommandProcessor();
            _channel = new FakeChannel();
            var mapper = new MyEventMessageMapper();
            _messagePump = new MessagePump<MyEvent>(_commandProcessor, mapper) { Channel = _channel, TimeoutInMilliseconds = 5000, RequeueCount = 3, UnacceptableMessageLimit = 3};

            var unacceptableMessage1 = new Message(new MessageHeader(Guid.NewGuid(), "MyTopic", MessageType.MT_UNACCEPTABLE), new MessageBody(""));
            var unacceptableMessage2 = new Message(new MessageHeader(Guid.NewGuid(), "MyTopic", MessageType.MT_UNACCEPTABLE), new MessageBody(""));
            var unacceptableMessage3 = new Message(new MessageHeader(Guid.NewGuid(), "MyTopic", MessageType.MT_UNACCEPTABLE), new MessageBody(""));

            _channel.Add(unacceptableMessage1);
            _channel.Add(unacceptableMessage2);
            _channel.Add(unacceptableMessage3);
        }

        [Test]
        public void When_An_Unacceptable_Message_Limit_Is_Reached()
        {
            var task = Task.Factory.StartNew(() => _messagePump.Run(), TaskCreationOptions.LongRunning);
            Task.Delay(1000).Wait();

            Task.WaitAll(new[] { task });

            //should_have_acknowledge_the_3_messages
            Assert.AreEqual(3, _channel.AcknowledgeCount);
            //should_dispose_the_input_channel
            Assert.True(_channel.DisposeHappened);
        }
    }
}