using CommonLib.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonLib
{


	[TestClass()]
	public class ObjectExTest
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

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion
		
		[Serializable]
		public class Person
		{
			public string FirstName = "JP";
			public string LastName = "Richardson";
			public int Age = 26;
			public ContactInfo Contact = new ContactInfo();
			
			[Serializable]
			public class ContactInfo
			{
				public int ZipCode = 68516;
			}
		}

		[TestMethod()]
		public void DeepCloneBinaryTest() {
			Person p1 = new Person();
			Person p2 = p1.DeepClone<Person>(ObjectEx.SerializationStrategy.Binary);
			Person p3 = p1;

			//ref comparison
			Assert.AreSame(p1, p3);
			Assert.AreNotSame(p1, p2);
			Assert.AreNotSame(p1.FirstName, p2.FirstName);
			Assert.AreNotSame(p1.LastName, p2.LastName);
			Assert.AreNotSame(p1.Contact, p2.Contact);

			//value comparison
			Assert.AreEqual(p1.FirstName, p2.FirstName);
			Assert.AreEqual(p1.LastName, p2.LastName);
			Assert.AreEqual(p1.Contact.ZipCode, p2.Contact.ZipCode);


			p1.FirstName = "Chris";
			Assert.AreNotEqual(p1.FirstName, p2.FirstName);
			Assert.AreEqual(p1.FirstName, p3.FirstName);
			Assert.AreSame(p1.FirstName, p3.FirstName);
		}

		[TestMethod()]
		public void DeepCloneXmlTest() {
			Person p1 = new Person();
			Person p2 = p1.DeepClone<Person>(ObjectEx.SerializationStrategy.Xml);
			Person p3 = p1;

			//ref comparison
			Assert.AreSame(p1, p3);
			Assert.AreNotSame(p1, p2);
			Assert.AreNotSame(p1.FirstName, p2.FirstName);
			Assert.AreNotSame(p1.LastName, p2.LastName);
			Assert.AreNotSame(p1.Contact, p2.Contact);

			//value comparison
			Assert.AreEqual(p1.FirstName, p2.FirstName);
			Assert.AreEqual(p1.LastName, p2.LastName);
			Assert.AreEqual(p1.Contact.ZipCode, p2.Contact.ZipCode);


			p1.FirstName = "Chris";
			Assert.AreNotEqual(p1.FirstName, p2.FirstName);
			Assert.AreEqual(p1.FirstName, p3.FirstName);
			Assert.AreSame(p1.FirstName, p3.FirstName);
		}
	}
}
