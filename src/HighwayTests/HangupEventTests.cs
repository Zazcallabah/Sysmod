using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class HangupEventTests
	{
		[TestMethod]
		public void TestHangupEventAction()
		{
			int hungup = 0;
			var station = new Station( 1, 0, 0, 10 );
			var e = new HangupEvent( station, 0, () => { hungup++; } );

			station.ClaimChannel( true );
			e.Action();

			Assert.AreEqual( 0, station.CurrentBusyChannels );
			Assert.AreEqual( 1, hungup );
		}
	}
}