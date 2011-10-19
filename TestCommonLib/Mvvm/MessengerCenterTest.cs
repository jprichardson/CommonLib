using CommonLib.Utility;
using CommonLib.Mvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;
using System.Dynamic;

namespace TestCommonLib
{

	[TestClass()]
	public class MessengerCenterTest
	{
		private TestContext testContextInstance;

		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		[TestInitialize()]
		public void MyTestInitialize() {

		}

		[TestCleanup()]
		public void MyTestCleanup() {

		}

		[TestMethod()]
		public void MessengerCenterObjTest() {
			var mc = new MessengerCenter();
			
			mc.Subscribe(this, "msg1", (msg) =>
			{
				Assert.AreEqual("msg1", msg.MessageString);
			});
			
			mc.Subscribe(this, "msg2", (msg) =>
			{
				Assert.AreEqual("msg2", msg.MessageString);
			});

			var nc = mc.Notify("msg1", null);
			Assert.AreEqual(1, nc);

			nc = mc.Notify("msg5", null);
			Assert.AreEqual(0, nc);

			bool didCall = false;
			mc.Subscribe(this, "msg2", (msg) =>
			{
				didCall = true;
				Assert.AreEqual("msg2", msg.MessageString);
			});

			Assert.IsFalse(didCall);

			nc = mc.Notify("msg2", null);
			Assert.AreEqual(2, nc);

			Assert.IsTrue(didCall);
		}

		
	}
}
