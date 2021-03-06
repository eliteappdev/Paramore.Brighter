﻿using System;
using NUnit.Framework;
using paramore.brighter.commandprocessor.messageviewer.Adaptors.API.Resources;
using paramore.brighter.commandprocessor.messageviewer.Ports.Domain;
using paramore.brighter.commandprocessor.messageviewer.Ports.ViewModelRetrievers;
using paramore.brighter.commandprocessor.viewer.tests.TestDoubles;

namespace paramore.brighter.commandprocessor.viewer.tests.Ports.MessageListViewModelRetrieverTests
{
    public class MessageListViewModelRetrieverFilterStoreErrorTests
    {
        private ViewModelRetrieverResult<MessageListModel, MessageListModelError> _result;
        private string storeName = "storeThatCannotGet";
        private MessageListViewModelRetriever _messageListViewModelRetriever;

        [SetUp]
        public void Establish()
        {
            var fakeStoreNotViewer = new FakeMessageStoreViewerWithGetException();
            var modelFactory = new FakeMessageStoreViewerFactory(fakeStoreNotViewer, storeName);
            _messageListViewModelRetriever = new MessageListViewModelRetriever(modelFactory);
        }

        [Test]
        public void When_fitlering_messages_with_store_that_cannot_get()
        {
            _result = _messageListViewModelRetriever.Filter(storeName, "term");

            //should_return_error
            var model = _result.Result;
            Assert.Null(model);
            Assert.True(_result.IsError);
            Assert.AreEqual(MessageListModelError.StoreMessageViewerGetException, _result.Error);
            Assert.NotNull(_result.Exception);
            Assert.IsInstanceOf<AggregateException>(_result.Exception);
        }

    }
}