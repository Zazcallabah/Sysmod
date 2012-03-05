using System;
using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class CallDataTests
	{
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

		[TestMethod]
		public void CallDataHoldsHashCode()
		{
			var st = new CallData( 1, 0, 0, 0 );
			var st2 = new CallData( 1, 0, 0, 0 );
			Assert.AreNotSame( st2, st );
			Assert.AreEqual( st.GetHashCode(), st2.GetHashCode() );
			var st3 = new CallData( 2, 0, 0, 0 );
			Assert.AreNotEqual( st.GetHashCode(), st3.GetHashCode() );
		}

		[TestMethod]
		public void CallDataHandlesEqualsNull()
		{
			var st = new CallData( 1, 0, 0, 0 );
			object o = null;
			Assert.IsFalse( st.Equals( null ) );
			Assert.IsTrue( st.Equals( (object) st ) );
			Assert.IsFalse( st.Equals( 33 ) );
			Assert.IsFalse( st.Equals( o ) );
		}

		[TestMethod]
		public void CreateCallDataUsingNegativeValuesThrows()
		{
			int exCount = 0;
			var ops = new Action[] { () => { new CallData( 400001, 0, 0, 0 ); } };

			foreach( Action action in ops )
				if( OperationThrowsException( action ) )
					exCount++;

			Assert.AreEqual( 1, exCount );
		}

		[TestMethod]
		public void PropertiesAreSetCorrectly()
		{
			uint starttime = 2;
			uint speed = 2;
			uint startposition = 13;
			uint duration = 20;

			uint endposition = 53;
			uint endtime = 22;

			var cd = new CallData( speed, startposition, duration, starttime );

			Assert.AreEqual( endposition, cd.EndPosition );
			Assert.AreEqual( endtime, cd.EndTime );
		}

		[TestMethod]
		public void GetPositionReturns()
		{
			uint starttime = 2;
			uint speed = 2;
			uint startposition = 13;
			uint duration = 20;

			var cd = new CallData( speed, startposition, duration, starttime );

			Assert.AreEqual( (uint) 13, cd.GetPositionForAbsoluteTime( 2 ) );
			Assert.AreEqual( cd.EndPosition, cd.GetPositionForAbsoluteTime( cd.EndTime ) );
			Assert.AreEqual( (uint) 33, cd.GetPositionForAbsoluteTime( 12 ) );
		}

		[TestMethod]
		public void GetTimeReturns()
		{
			uint starttime = 2;
			uint speed = 2;
			uint startposition = 13;
			uint duration = 20;

			var cd = new CallData( speed, startposition, duration, starttime );

			Assert.AreEqual( (uint) 2, cd.GetAbsoluteTimeForPosition( 13 ) );
			Assert.AreEqual( cd.EndTime, cd.GetAbsoluteTimeForPosition( cd.EndPosition ) );
			Assert.AreEqual( (uint) 12, cd.GetAbsoluteTimeForPosition( 33 ) );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void GetInvalidPositionThrows()
		{
			var cd = new CallData( 1, 0, 10, 0 );
			cd.GetPositionForAbsoluteTime( 30 );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void GetNegativePositionThrows()
		{
			var cd = new CallData( 1, 0, 10, 10 );
			cd.GetPositionForAbsoluteTime( 5 );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void GetInvalidTimeThrows()
		{
			var cd = new CallData( 1, 0, 10, 0 );
			cd.GetAbsoluteTimeForPosition( 30 );
		}

		[TestMethod]
		[ExpectedException( typeof( ArgumentOutOfRangeException ) )]
		public void GetNegativeTimeThrows()
		{
			var cd = new CallData( 1, 50, 10, 0 );
			cd.GetAbsoluteTimeForPosition( 30 );
		}

		[TestMethod]
		public void TestEquals()
		{
			var cd1 = new CallData( 1, 1, 1, 1 );
			var cd2 = new CallData( 2, 2, 2, 2 );
			var cd3 = new CallData( 1, 1, 1, 1 );

			Assert.AreEqual( cd1, cd3 );
			Assert.AreNotSame( cd3, cd1 );
			Assert.AreSame( cd1, cd1 );
			Assert.AreNotEqual( cd1, cd2 );
		}

		[TestMethod]
		public void TestWrap()
		{
			var cd1 = new CallData( 1, 10, 40, 0 );
			CallData cd2 = cd1.Wrap( 10 );

			Assert.AreNotEqual( cd1, cd2 );

			Assert.AreEqual( cd1.Speed, cd2.Speed );
			Assert.AreEqual( (uint) 0, cd2.StartPosition );
			Assert.AreEqual( (uint) 10, cd2.StartTime );
			Assert.AreEqual( cd1.Duration - 10, cd2.Duration );
		}
	}
}