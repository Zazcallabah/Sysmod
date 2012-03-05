using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class EventQueueTests
	{
		[TestMethod]
		public void SimpleSimulationOneStep()
		{
			var calls = new[] { new CallData( 1, 0, 10, 0 ), new CallData( 2, 2, 5, 3 ) };
			EventQueue eq = CreateQueue( calls, 2, 10, 1, 0 );
			eq.AddCallEvent( 0 );

			Assert.AreEqual( (uint) 0, eq.PerformNextEvent() );
			Assert.AreEqual( 2, eq._innerQueue.Values.Count );
		}

		[TestMethod]
		public void SimpleSimulationSeveralSteps()
		{
			var calls = new[]
			{
				new CallData( 1, 0, 1000, 0 ), new CallData( 2, 2000, 5000, 3000 ), new CallData( 1, 8000, 3000, 9000 ),
				new CallData( 10, 1000, 1000, 100000 )
			};
			EventQueue eq = CreateQueue( calls, 2, 10000, 1, 0 );

			eq.Poke();
			do
			{
			} while( eq.PerformNextEvent() < 100000 );

			Assert.AreEqual( 2, eq._innerQueue.Values.Count );
		}

		[TestMethod]
		public void SimulationHandlesStationEdgesRoundingErrors()
		{
			var calls = new[]
			{
				new CallData( 1, 0, 50, 0 ), new CallData( 2, 2, 5, 60 ), new CallData( 1, 8, 3, 90 ),
				new CallData( 10, 1, 1, 100 )
			};
			EventQueue eq = CreateQueue( calls, 7, 25, 1, 0 );

			eq.Poke();

			do
			{
			} while( eq.PerformNextEvent() < 60 );

			Assert.AreEqual( 2, eq._innerQueue.Values.Count );
		}

		[TestMethod]
		public void SimulationHandlesCallsWithSameStarttime()
		{
			var calls = new[]
			{
				new CallData( 1, 0, 50, 0 ), new CallData( 1, 0, 20, 0 ), new CallData( 1, 10, 10, 10 ),
				new CallData( 10, 1, 1, 100 )
			};
			EventQueue eq = CreateQueue( calls, 10, 100, 1, 0 );

			eq.Poke();

			do
			{
			} while( eq.PerformNextEvent() < 60 );

			Assert.AreEqual( 2, eq._innerQueue.Values.Count );
		}

		static EventQueue CreateQueue( CallData[] data, uint stationcount, uint highwaylength, uint channels, uint reserved )
		{
			return new EventQueue( new DataGathererStub(), new CallGen( data ), stationcount, highwaylength, channels, reserved );
		}
	}

	public class CallGen : IRandomCallGenerator
	{
		#region Private fields
		readonly CallData[] _datas;
		int _count;
		#endregion

		public CallGen( CallData[] datas )
		{
			_count = 0;
			_datas = datas;
		}

		#region IRandomCallGenerator Members
		public CallData GenerateRandomCall( uint lastCallStartTime )
		{
			return _datas[_count++ % _datas.Length];
		}
		#endregion
	}

	public class DataGathererStub : IDataGatherer
	{
		#region IDataGatherer Members
		public void Record()
		{
		}

		public void SignalCallStarted()
		{
		}

		public void SignalCallBlocked()
		{
		}

		public void SignalCallDropped()
		{
		}

		public void SignalCallHangup()
		{
		}
		#endregion
	}
}