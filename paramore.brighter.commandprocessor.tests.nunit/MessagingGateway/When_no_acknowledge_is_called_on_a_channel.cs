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
using FakeItEasy;
using NUnit.Framework;

namespace paramore.brighter.commandprocessor.tests.nunit.MessagingGateway
{
    [TestFixture]
    public class ChannelNackTests
    {
        private IAmAChannel _channel;
        private IAmAMessageConsumer _gateway;
        private Message _receivedMessage;

        [SetUp]
        public void Establish()
        {
            _gateway = A.Fake<IAmAMessageConsumer>();

            _channel = new Channel("test", _gateway);

            _receivedMessage = new Message(
                new MessageHeader(Guid.NewGuid(), "key", MessageType.MT_EVENT),
                new MessageBody("a test body"));

            _receivedMessage.SetDeliveryTag(12345UL);
        }

        [Test]
        public void When_No_Acknowledge_Is_Called_On_A_Channel()
        {
            _channel.Reject(_receivedMessage);

            //_should_ackonwledge_the_message
            A.CallTo(() => _gateway.Reject(_receivedMessage, true)).MustHaveHappened();
        }
    }
}