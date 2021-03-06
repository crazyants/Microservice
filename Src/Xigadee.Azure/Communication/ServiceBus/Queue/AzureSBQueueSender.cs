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

#region using
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
#endregion
namespace Xigadee
{
    /// <summary>
    /// This is the Azure service bus sender component used to transmit messages to a remote service.
    /// </summary>
    [DebuggerDisplay("AzureSBQueueSender: {ChannelId}")]
    public class AzureSBQueueSender : AzureSBSenderBase<QueueClient,BrokeredMessage>
    {
        #region ClientCreate()
        /// <summary>
        /// This override sets the transmit options for the client.
        /// </summary>
        /// <returns>Returns the client.</returns>
        protected override AzureClientHolder<QueueClient, BrokeredMessage> ClientCreate(SenderPartitionConfig partition)
        {
            var client = base.ClientCreate(partition);

            client.Type = "Queue Sender";
            client.Name = mPriorityClientNamer(AzureConn.ConnectionName, partition.Priority);

            client.AssignMessageHelpers();

            client.FabricInitialize = () => AzureConn.QueueFabricInitialize(client.Name);

            //Set the method that creates the client.
            client.ClientCreate = () => QueueClient.CreateFromConnectionString(AzureConn.ConnectionString, client.Name);

            //We have to do this due to the stupid inheritance rules for Azure Service Bus.
            client.MessageTransmit = async (b) => await client.Client.SendAsync(b);

            return client;
        } 
        #endregion
    }
}
