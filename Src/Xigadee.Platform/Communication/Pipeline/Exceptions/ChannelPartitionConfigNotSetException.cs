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

namespace Xigadee
{
    /// <summary>
    /// This exception is throw when you try and set the channel partitions and they are already set. 
    /// Check that autosetPartition01 is set to false when creating the channel.
    /// </summary>
    public class ChannelPartitionConfigNotSetException: ChannelPartitionConfigBaseException
    {
        /// <summary>
        /// This is the default constructor.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        public ChannelPartitionConfigNotSetException(string channelId)
            : base(channelId, $"Channel partitions have not been set for channel '{channelId}'. If autosetPartition01 is set to false, check that the AttachPriorityPartition is before adding the listener.")
        {
        }

        /// <summary>
        /// THis is the first priority that caused the exception.
        /// </summary>
        public int ChannelPriority { get; }
    }
}
