using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SimHighway;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class ReplicationTests
	{
		#region Private fields
		readonly MockRepository _mocks;
		#endregion

		public ReplicationTests()
		{
			_mocks = new MockRepository();
		}

		//[TestMethod]
		//public void EngineCallsReplication()
		//{
		//    uint[] events = { 0, 500, 995, 1000, 4000, 5000, 9999, 1000000 };
		//    uint startlength = 1000;
		//    uint replength = 10000;
		//    var dg = _mocks.DynamicMock<IDataGatherer>();
		//    var eqfactory = _mocks.DynamicMock<IEventQueueFactory>();
		//    var eq = _mocks.DynamicMock<IEventQueue>();

		//    SetupResult.For( eqfactory.Create( dg ) ).Return( eq );

		//    Expect.Call( eq.Poke ).Repeat.Once();
		//    //Expect.Call( eq.PerformNextEvent() ).Repeat.Times( events.Length ).;

		//    _mocks.ReplayAll();

		//    var rep = new Replication( replength, dg, eqfactory, startlength );
		//    Assert.AreEqual( ReplicationState.NotStarted, rep.CurrentState );
		//    rep.Run();
		//    Assert.AreEqual( ReplicationState.Finished, rep.CurrentState );
		//    _mocks.VerifyAll();
		//}
	}
}