using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class HandoverEventTests
	{
		[TestMethod]
		public void TestHandoverEventActionDropped()
		{
			int dropped = 0;
			int createdhandover = 0;
			int createdend = 0;
			var data = new CallData( 3, 5, 11, 0 );
			var tostation = new Station( 0, 0, 0, 10 );
			var fromstation = new Station( 1, 0, 0, 10 );
			fromstation.ClaimChannel( true );

			var e = new HandoverEvent(
				fromstation,
				tostation,
				() => { dropped++; },
				( d, cd ) =>
				{
					createdend++;
					Assert.AreEqual( data, cd );
				},
				( d, cd ) =>
				{
					createdhandover++;
					Assert.AreEqual( data, cd );
				},
				0,
				data );

			e.Action();

			Assert.AreEqual( 1, dropped );
			Assert.AreEqual( 0, createdend );
			Assert.AreEqual( 0, createdhandover );
		}

		[TestMethod]
		public void TestHandoverEventActionCallEnd()
		{
			int dropped = 0;
			int createdhandover = 0;
			int createdend = 0;

			var data = new CallData( 1, 5, 10, 0 );
			var tostation = new Station( 1, 0, 10, 10 );

			var fromstation = new Station( 1, 0, 0, 10 );
			fromstation.ClaimChannel( true );

			var e = new HandoverEvent(
				fromstation,
				tostation,
				() => { dropped++; },
				( d, cd ) =>
				{
					createdend++;
					Assert.AreEqual( (uint) 10, d );
					Assert.AreEqual( data, cd );
				},
				( d, cd ) =>
				{
					createdhandover++;
					Assert.AreEqual( data, cd );
				},
				5,
				data );

			e.Action();

			Assert.AreEqual( 0, dropped );
			Assert.AreEqual( 1, createdend );
			Assert.AreEqual( 0, createdhandover );
		}

		[TestMethod]
		public void TestHandoverEventActionCallHandover()
		{
			int dropped = 0;
			int createdhandover = 0;
			int createdend = 0;

			var data = new CallData( 1, 5, 20, 0 );
			var tostation = new Station( 1, 0, 10, 10 );

			var fromstation = new Station( 1, 0, 0, 10 );
			fromstation.ClaimChannel( true );

			var e = new HandoverEvent(
				fromstation,
				tostation,
				() => { dropped++; },
				( d, cd ) => { createdend++; },
				( d, cd ) =>
				{
					createdhandover++;
					Assert.AreEqual( data, cd );
					Assert.AreEqual( (uint) 15, d );
				},
				5,
				data );

			e.Action();

			Assert.AreEqual( 0, dropped );
			Assert.AreEqual( 0, createdend );
			Assert.AreEqual( 1, createdhandover );
		}
	}
}