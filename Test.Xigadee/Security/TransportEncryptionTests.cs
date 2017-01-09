﻿using System;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xigadee;

namespace Test.Xigadee
{
    [TestClass]
    public class TransportEncryptionTests
    {
        [TestClass]
        public class CommunicationBridgeTests
        {
            private static string CreateSalt(int size)
            {
                //Generate a cryptographic random number.
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] buff = new byte[size/8];
                rng.GetBytes(buff);

                // Return a Base64 string representation of the random number.
                return Convert.ToBase64String(buff);
            }

            [TestMethod]
            public void TestMethod1()
            {
                var bridgeOut = new CommunicationBridge(CommunicationBridgeMode.RoundRobin);
                var bridgein = new CommunicationBridge(CommunicationBridgeMode.Broadcast);

                var key = CreateSalt(128);

                var encOut = new AesEncryptionHandler(key, 128);
                var encIn = new AesEncryptionHandler(key, 128);


                PersistenceMessageInitiator<Guid, SecureMe> init;
                DebugMemoryDataCollector memp1, memp2;

                var p1 = new MicroservicePipeline("Sender")
                    .AddEncryptionHandler("rogue1",encOut)
                    .AdjustPolicyCommunication((p) => p.BoundaryLoggingActiveDefault = true)
                    .AddDataCollector((c) => new DebugMemoryDataCollector(), (c) => memp1 = c)
                    .AddChannelOutgoing("crequest")
                        .AttachTransportPayloadEncryption("rogue1")
                        .AttachSender(bridgeOut.GetSender())
                        .Revert()
                    .AddChannelIncoming("cresponse")
                        .AttachListener(bridgein.GetListener())
                        .AttachPersistenceMessageInitiator(out init, "crequest")
                        ;

                var p2 = new MicroservicePipeline("Receiver")
                    .AddEncryptionHandler("rogue2", encIn)
                    .AdjustPolicyCommunication((p) => p.BoundaryLoggingActiveDefault = true)
                    .AddDataCollector((c) => new DebugMemoryDataCollector(), (c) => memp2 = c)
                    .AddChannelIncoming("crequest")
                        .AttachTransportPayloadDecryption("rogue2")
                        .AttachListener(bridgeOut.GetListener())
                        .AttachCommand(new PersistenceManagerHandlerMemory<Guid, SecureMe>((e) => e.Id, (s) => new Guid(s)))
                        .Revert()
                    .AddChannelOutgoing("cresponse")
                        .AttachSender(bridgein.GetSender())
                        ;

                p1.Start();
                p2.Start();

                int check1 = p1.ToMicroservice().Commands.Count();
                int check2 = p2.ToMicroservice().Commands.Count();

                var entity = new SecureMe() { Message = "Momma" };
                var rs = init.Create(entity, new RepositorySettings() { WaitTime = TimeSpan.FromMinutes(5) }).Result;
                var rs2 = init.Read(entity.Id).Result;

                Assert.IsTrue(rs2.IsSuccess);
                Assert.IsTrue(rs2.Entity.Message == "Momma");
            }

            private void CommunicationBridgeTests_OnExecuteBegin(object sender, Microservice.TransmissionPayloadState e)
            {

            }
        }

        public class SecureMe
        {
            public Guid Id { get; set; } = Guid.NewGuid();

            public string Message { get; set; }
        }
    }
}
