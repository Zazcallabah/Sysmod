using System.Globalization;
using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimHighway;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class DataGathererTests
	{
		[TestMethod]
		public void DataGathererToStringNotRecording()
		{
			var d = new DataGatherer( 2 );
			d.SignalCallStarted();
			d.SignalCallDropped();
			d.SignalCallStarted();
			d.SignalCallDropped();
			d.SignalCallStarted();
			d.SignalCallDropped();
			d.SignalCallStarted();
			d.SignalCallBlocked();
			d.SignalCallStarted();
			d.SignalCallBlocked();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			string s = string.Format( CultureInfo.InvariantCulture,
				@"average total calls = {0:F4}
average blocked calls = {1:F4}
average dropped calls = {2:F4}
percent blocked calls = {3:F4}%
percent dropped calls = {4:F4}%", 5, 1, 1.5, 20, 30 );
			;

			Assert.AreEqual( s, d.ToString() );
		}

		[TestMethod]
		public void DataGathererToStringRecording()
		{
			var d = new DataGatherer( 10 );
			d.Record();
			d.SignalCallStarted();
			d.SignalCallDropped();
			d.SignalCallStarted();
			d.SignalCallDropped();
			d.SignalCallStarted();
			d.SignalCallDropped();
			d.SignalCallStarted();
			d.SignalCallBlocked();
			d.SignalCallStarted();
			d.SignalCallBlocked();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			d.SignalCallStarted();
			d.SignalCallHangup();
			string s = string.Format( CultureInfo.InvariantCulture,
				@"replication number = 10
total calls = 10
blocked calls = 2
dropped calls = 3
percent blocked calls = {0:F4}%
percent dropped calls = {1:F4}%
-------------------------------------------
", 20, 30 );

			Assert.AreEqual( s, d.ToString() );
		}

		[TestMethod]
		public void DataGathererAdds()
		{
			var d1 = new DataGatherer( 1 );
			var d2 = new DataGatherer( 2 );
			d1.SignalCallBlocked();
			d1.SignalCallBlocked();
			d1.SignalCallBlocked();
			d2.SignalCallBlocked();
			d2.SignalCallBlocked();
			DataGatherer d3 = d1 + d2;

			Assert.AreEqual( (ulong) 5, d3.CallBlocked );
		}

		[TestMethod]
		public void DataGathererAddsInterface()
		{
			var d1 = new DataGatherer( 1 );
			IDataGatherer d2 = new DataGatherer( 2 );
			d1.SignalCallBlocked();
			d1.SignalCallBlocked();
			d1.SignalCallBlocked();
			d2.SignalCallBlocked();
			d2.SignalCallBlocked();
			d1 += d2;

			Assert.AreEqual( (ulong) 5, d1.CallBlocked );
		}

		[TestMethod]
		public void DataGathererRecords()
		{
			var d = new DataGatherer( 1 );
			d.SignalCallBlocked();
			d.SignalCallBlocked();
			d.SignalCallBlocked();
			d.Record();
			d.SignalCallBlocked();
			d.SignalCallBlocked();
			Assert.AreEqual( (ulong) 2, d.CallBlocked );
		}

		[TestMethod]
		public void DataGathererHoldsData()
		{
			var d1 = new DataGatherer( 1 );

			d1.SignalCallBlocked();
			d1.SignalCallDropped();
			d1.SignalCallDropped();
			d1.SignalCallDropped();
			d1.SignalCallHangup();
			d1.SignalCallHangup();

			d1.SignalCallStarted();
			d1.SignalCallStarted();
			d1.SignalCallStarted();
			d1.SignalCallStarted();

			Assert.AreEqual( (ulong) 1, d1.CallBlocked );
			Assert.AreEqual( (ulong) 2, d1.CallHangup );
			Assert.AreEqual( (ulong) 3, d1.CallDropped );
			Assert.AreEqual( (ulong) 4, d1.CallStarted );
		}
	}
}