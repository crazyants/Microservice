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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xigadee
{
    /// <summary>
    /// This is the default statistics class.
    /// </summary>
    public class CommunicationStatistics: StatusBase
    {
        #region Name
        /// <summary>
        /// Name override so that it gets serialized at the top of the JSON data.
        /// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }

            set
            {
                base.Name = value;
            }
        } 
        #endregion

        /// <summary>
        /// This list of active clients and their poll statistics.
        /// </summary>
        public ClientPriorityCollectionStatistics Active { get; set; }
        /// <summary>
        /// The senders collection.
        /// </summary>
        public List<MessagingServiceStatistics> Senders { get; set; }
        /// <summary>
        /// The listeners collection.
        /// </summary>
        public List<MessagingServiceStatistics> Listeners { get; set; }
    }
}
