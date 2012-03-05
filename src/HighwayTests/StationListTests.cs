using System;
using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class StationListTests
	{
		[TestMethod]
		public void SetupStationsCorrectly()
		{
			var st = new StationList( 5, 50, 1, 0 );
			var arr = new[]
			{
				st.GetStationForPosition( 5 ), st.GetStationForPosition( 15 ), st.GetStationForPosition( 25 ),
				st.GetStationForPosition( 36 ), st.GetStationForPosition( 45 )
			};

			Assert.AreEqual( (uint) 0, arr[0].StartPosition );
			Assert.AreEqual( (uint) 10, arr[1].StartPosition );
			Assert.AreEqual( (uint) 20, arr[2].StartPosition );
			Assert.AreEqual( (uint) 30, arr[3].StartPosition );
			Assert.AreEqual( (uint) 40, arr[4].StartPosition );
			Assert.AreEqual( (uint) 10, arr[0].EndPosition );
			Assert.AreEqual( (uint) 20, arr[1].EndPosition );
			Assert.AreEqual( (uint) 30, arr[2].EndPosition );
			Assert.AreEqual( (uint) 40, arr[3].EndPosition );
			Assert.AreEqual( (uint) 50, arr[4].EndPosition );
		}

		[TestMethod]
		public void FirstStationIsFirst()
		{
			var st = new StationList( 5, 50, 1, 0 );
			Station first = st.First;

			Assert.AreSame( first, st.GetStationForPosition( 0 ) );
		}

		[TestMethod]
		public void StationListReturnsFirstStationWhenWrapping()
		{
			var st = new StationList( 5, 50, 1, 0 );
			Station first = st.GetStationForPosition( 51, true );

			Assert.AreSame( first, st.GetStationForPosition( 0 ) );
		}

		[TestMethod]
		public void StationsAreCreatedAtProperPositions()
		{
			var st = new StationList( 2, 10000, 1, 0 );
			Station first = st.GetStationForPosition( 0 );
			Station third = st.GetStationForPosition( 4999 );
			Station second = st.GetStationForPosition( 5000 );
			Station fourth = st.GetStationForPosition( 9000 );

			Assert.AreSame( first, third );
			Assert.AreSame( second, fourth );
		}

		[TestMethod]
		public void CreateStationListThrowsWhenGivenInvalidData()
		{
			int exCount = 0;
			var ops = new Action[]
			{
				() => { new StationList( 5, 5, 5, 6 ); }, () => { new StationList( 5, 0, 0, 0 ); },
				() => { new StationList( 0, 5, 1, 1 ); }
			};

			foreach( Action action in ops )
				if( OperationThrowsException( action ) )
					exCount++;

			Assert.AreEqual( 3, exCount );
		}

		static bool OperationThrowsException( Action operation )
		{
			try
			{
				operation();
			}
			catch( Exception )
			{
				return true;
			}
			return false;
		}
	}
}