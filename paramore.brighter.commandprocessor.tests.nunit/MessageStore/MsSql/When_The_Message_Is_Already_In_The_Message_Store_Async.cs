﻿#region Licence

/* The MIT License (MIT)
Copyright © 2014 Francesco Pighi <francesco.pighi@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the “Software”), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

#endregion

using System;
using System.IO;
using Nito.AsyncEx;
using NUnit.Framework;
using paramore.brighter.commandprocessor.messagestore.mssql;

namespace paramore.brighter.commandprocessor.tests.nunit.MessageStore.MsSql
{
    [Category("MSSQL")]
    [TestFixture]
    public class MsSqlMessageStoreMessageAlreadyExistsAsyncTests
    {
        private Exception _exception;
        private Message _messageEarliest;
        private MsSqlMessageStore _sqlMessageStore;
        private MsSqlTestHelper _msSqlTestHelper;

        [SetUp]
        public void Establish()
        {
            _msSqlTestHelper = new MsSqlTestHelper();
            _msSqlTestHelper.SetupMessageDb();

            _sqlMessageStore = new MsSqlMessageStore(_msSqlTestHelper.MessageStoreConfiguration);
            _messageEarliest = new Message(new MessageHeader(Guid.NewGuid(), "test_topic", MessageType.MT_DOCUMENT),
                new MessageBody("message body"));
            AsyncContext.Run(async () => await _sqlMessageStore.AddAsync(_messageEarliest));
        }

        [Test]
        public void When_The_Message_Is_Already_In_The_Message_Store_Async()
        {
            _exception = Catch.Exception(() => AsyncContext.Run(async () => await _sqlMessageStore.AddAsync(_messageEarliest)));

            //_should_ignore_the_duplcate_key_and_still_succeed
            Assert.Null(_exception);
        }

        [TearDown]
        public void Cleanup()
        {
            CleanUpDb();
        }

        private void CleanUpDb()
        {
            _msSqlTestHelper.CleanUpDb();
        }
    }
}
