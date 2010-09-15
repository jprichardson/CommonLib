using CommonLib.Mvvm.Progress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace TestCommonLib
{

	[TestClass()]
	public class ProgressViewModelTest
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


		[TestMethod()]
		public void ProgressTaskViewModelTest() {
			ProgressViewModel target = new ProgressTaskViewModel("Some task...");
			ProgressViewModelTestRun(target);

		}

		[TestMethod()]
		public void ProgressWorkerViewModelTest() {
			ProgressViewModel target = new ProgressWorkerViewModel("Some task...");
			ProgressViewModelTestRun(target);

		}

		private void ProgressViewModelTestRun(ProgressViewModel pvm) {
			Assert.AreEqual("Some task...", pvm.ProgressText);

			//Test basic thread operation
			int count = 0;
			pvm.ActionWork = (p) => { Thread.Sleep(1000); count = 100; };
			pvm.ExecuteCommand.Execute(null);

			Assert.AreNotEqual(100, count);
			pvm.Wait();
			Assert.AreEqual(100, count);

			//Test action on completion
			count = 0;
			pvm.ActionWork = (p) => { Thread.Sleep(1000); };
			pvm.ActionComplete = () => { count = 100; };
			pvm.ExecuteCommand.Execute(null);

			Assert.AreNotEqual(100, count);
			pvm.Wait();
			Assert.AreEqual(100, count);

			//Test progress = this should fail
			pvm.ReportsProgress = false;
			bool exceptionHappend = false;
			pvm.ActionWork = (p) =>
			{
				try {
					p.ReportProgress(5);
				}
				catch { exceptionHappend = true; }
			};
			pvm.ExecuteCommand.Execute(null);

			pvm.Wait();
			Assert.IsTrue(exceptionHappend);

			//Test progress
			pvm.ReportsProgress = true;
			pvm.ActionWork = (p) => { 
				p.ReportProgress(50);
				Thread.Sleep(1000);
			};

			int progress = 0;
			pvm.ActionProgressUpdate = (x) =>
			{
				progress = x;
			};
			pvm.ExecuteCommand.Execute(null);

			Assert.AreNotEqual(50, progress);
			pvm.Wait();
			Assert.AreEqual(50, progress);
		}
	}
}
