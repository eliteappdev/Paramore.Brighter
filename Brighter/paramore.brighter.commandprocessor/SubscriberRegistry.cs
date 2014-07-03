﻿#region Licence
/* The MIT License (MIT)
Copyright © 2014 Ian Cooper <ian_hammond_cooper@yahoo.co.uk>

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace paramore.brighter.commandprocessor
{
    public class SubscriberRegistry : IAmASubscriberRegistry, IEnumerable<KeyValuePair<Type, Type>>
    {
        private readonly Dictionary<Type, Type> registeredSubscribers = new Dictionary<Type, Type>(); 
        public IEnumerable<IHandleRequests<TRequest>> Get<TRequest>() where TRequest : class, IRequest
        {
            return registeredSubscribers
                .Where(registryEntry => registryEntry.Key == typeof (TRequest))
                .Select(registryEntry => registryEntry.Value)
                .Cast<IHandleRequests<TRequest>>();
        }

        //Support object initializer syntax
        public void Add(Type requestType, Type handlerType)
        {
            registeredSubscribers.Add(requestType, handlerType);
        }

        public void Register<TRequest, TImplementation>() where TRequest: class, IRequest where TImplementation: class, IHandleRequests<TRequest>
        {
            registeredSubscribers.Add(typeof(TRequest), typeof(TImplementation));
        }

        public IEnumerator<KeyValuePair<Type, Type>> GetEnumerator()
        {
            return registeredSubscribers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}