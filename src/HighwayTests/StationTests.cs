using System;
using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class StationTests
	{
		[TestMethod]
		public void StationHandlesEquals()
		{
			var st = new Station( 0, 0, 2, 1 );
			var st2 = new Station( 0, 0, 2, 1 );
			var st3 = new Station( 0, 0, 2, 3 );

			Assert.AreEqual( st, st2 );
			Assert.AreNotEqual( st, st3 );
			Assert.AreNotSame( st, st2 );
		}

		[TestMethod]
		public void StationIsCreatedAtProperPosition()
		{
			var st = new Station( 0, 0, 2, 1 );

			Assert.AreEqual( (uint) 2, st.StartPosition );
			Assert.AreEqual( (uint) 3, st.EndPosition );
		}

		[TestMethod]
		public void StationIsCreatedWithCorrectNumberOfChannels()
		{
			var st = new Station( 2, 0, 1, 1 );

			Assert.AreEqual( 0, st.CurrentBusyChannels );
			Assert.IsTrue( st.ClaimChannel( false ) );
			Assert.AreEqual( 1, st.CurrentBusyChannels );
			Assert.IsTrue( st.ClaimChannel( false ) );
			Assert.AreEqual( 2, st.CurrentBusyChannels );
			Assert.IsFalse( st.ClaimChannel( false ) );
			st.ReleaseChannel();
			st.ReleaseChannel();
			Assert.AreEqual( 0, st.CurrentBusyChannels );
		}

		[TestMethod]
		public void StationIsCreatedWithCorrectNumberOfReservedChannels()
		{
			var st = new Station( 2, 1, 1, 1 );

			Assert.AreEqual( 0, st.CurrentBusyChannels );
			Assert.IsTrue( st.ClaimChannel( false ) );
			Assert.AreEqual( 1, st.CurrentBusyChannels );
			Assert.IsFalse( st.ClaimChannel( false ) );
			Assert.IsTrue( st.ClaimChannel( true ) );
			Assert.AreEqual( 2, st.CurrentBusyChannels );
			Assert.IsFalse( st.ClaimChannel( false ) );
			Assert.IsFalse( st.ClaimChannel( true ) );
			st.ReleaseChannel();
			st.ReleaseChannel();
			Assert.AreEqual( 0, st.CurrentBusyChannels );
		}

		[TestMethod]
		public void StationReturnsForPositionInRange()
		{
			var st = new Station( 0, 0, 2000, 1000 );
			Assert.IsTrue( st.PositionIsInRange( 2000 ) );
			Assert.IsTrue( st.PositionIsInRange( 2500 ) );
			Assert.IsTrue( st.PositionIsInRange( 2900 ) );
			Assert.IsTrue( st.PositionIsInRange( 2999 ) );
			Assert.IsFalse( st.PositionIsInRange( 1000 ) );
			Assert.IsFalse( st.PositionIsInRange( 0 ) );
			Assert.IsFalse( st.PositionIsInRange( 3000 ) );
			Assert.IsFalse( st.PositionIsInRange( 1999 ) );
		}

		[TestMethod]
		public void CreateStationThrowsWhenGivenInvalidData()
		{
			int exCount = 0;
			var ops = new Action[] { () => { new Station( 0, 1, 0, 0 ); }, };

			foreach( Action action in ops )
				if( OperationThrowsException( action ) )
					exCount++;

			Assert.AreEqual( 1, exCount );
		}

		[TestMethod]
		public void StationHoldsHashCode()
		{
			var st = new Station( 1, 0, 0, 0 );
			var st2 = new Station( 1, 0, 0, 0 );
			Assert.AreNotSame( st2, st );
			Assert.AreEqual( st.GetHashCode(), st2.GetHashCode() );
			var st3 = new Station( 2, 0, 0, 0 );
			Assert.AreNotEqual( st.GetHashCode(), st3.GetHashCode() );
		}

		[TestMethod]
		public void StationHandlesEqualsNull()
		{
			var st = new Station( 1, 0, 0, 0 );
			object o = null;
			Assert.IsFalse( st.Equals( null ) );
			Assert.IsTrue( st.Equals( (object) st ) );
			Assert.IsFalse( st.Equals( 33 ) );
			Assert.IsFalse( st.Equals( o ) );
		}

		[TestMethod]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void StationThrowsWhenReleasingUnclaimedChannels()
		{
			var st = new Station( 0, 0, 0, 0 );
			st.ReleaseChannel();
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