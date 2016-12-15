﻿#region Copyright
// Copyright Hitachi Consulting
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//    http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;

namespace Xigadee
{
    /// <summary>
    /// This interface is used by the configuration pipeline to add necessary components to the microservice.
    /// </summary>
    public interface IMicroserviceConfigure
    {
        /// <summary>
        /// This method registers a channel with the Microservice.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <returns>Returns the channel registered.</returns>
        Channel RegisterChannel(Channel channel);

        ICommand RegisterCommand(ICommand command);

        IEventSourceComponent RegisterEventSource(IEventSourceComponent eventSource);
        ILogger RegisterLogger(ILogger logger);
        IBoundaryLoggerComponent RegisterBoundaryLogger(IBoundaryLoggerComponent logger);
        IDataCollectorComponent RegisterDataCollector(IDataCollectorComponent logger);

        IPayloadSerializer RegisterPayloadSerializer(IPayloadSerializer serializer);
        void ClearPayloadSerializers();

        IListener RegisterListener(IListener listener);
        ISender RegisterSender(ISender sender);

        ISharedService SharedServices { get; }
    }
}