using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class EventBaseTests
	{
		[TestMethod]
		public void EventBaseHoldsTime()
		{
			var e = new EventBaseStub( 1 );

			Assert.AreEqual( (uint) 1, e.TriggerTime );
		}
	}

	internal class EventBaseStub : EventBase
	{
		public EventBaseStub( uint t ) : base( t )
		{
		}

		public override void Action()
		{
		}
	}
}