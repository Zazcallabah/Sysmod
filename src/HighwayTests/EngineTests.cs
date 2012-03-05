using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomContainer;
using Rhino.Mocks;
using SimHighway;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class EngineTests
	{
		#region Private fields
		readonly MockRepository _mocks;
		#endregion

		public EngineTests()
		{
			_mocks = new MockRepository();
		}

		[TestMethod]
		public void EngineCallsReplication()
		{
			uint repcount = 10;
			var reps = _mocks.DynamicMock<IReplicationFactory>();
			var rands = _mocks.DynamicMock<IRandomFactory>();
			var rep = _mocks.DynamicMock<IReplication>();
			SetupResult.For( rep.ReplicationData ).Return( new DataGatherer( 0 ) );
			Expect.Call( reps.Create( null, null ) ).IgnoreArguments().Repeat.Times( (int) repcount ).Return( rep );
			Expect.Call( rep.Run ).Repeat.Times( (int) repcount );
			_mocks.ReplayAll();

			var se = new SimulationEngine( 5, 1000, repcount, 10, 2, o => { }, rands, reps, 1 );
			se.Start();

			_mocks.VerifyAll();
		}
	}
}