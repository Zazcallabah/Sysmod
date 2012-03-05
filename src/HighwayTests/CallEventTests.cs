using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class CallEventTests
	{
		[TestMethod]
		public void TestCreateEventActionBlocked()
		{
			int blocked = 0;
			int started = 0;
			int created = 0;
			int createdhandover = 0;
			int createdend = 0;

			var data = new CallData( 1, 5, 11, 0 );
			var station = new Station( 0, 0, 0, 10 );

			var e = new CallEvent(
				station,
				data,
				() => { blocked++; },
				() => { started++; },
				( d ) =>
				{
					created++;
					Assert.AreEqual( (uint) 0, d );
				},
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
				0 );

			e.Action();

			Assert.AreEqual( 1, blocked );
			Assert.AreEqual( 1, created );
			Assert.AreEqual( 1, started );
			Assert.AreEqual( 0, createdend );
			Assert.AreEqual( 0, createdhandover );
		}

		[TestMethod]
		public void TestCreateEventActionEnd()
		{
			int blocked = 0;
			int started = 0;
			int created = 0;
			int createdhandover = 0;
			int createdend = 0;

			var data = new CallData( 1, 5, 3, 0 );
			var station = new Station( 2, 0, 0, 10 );

			var e = new CallEvent(
				station,
				data,
				() => { blocked++; },
				() => { started++; },
				( d ) =>
				{
					created++;
					Assert.AreEqual( (uint) 0, d );
				},
				( d, cd ) =>
				{
					createdend++;
					Assert.AreEqual( (uint) 3, d );
					Assert.AreEqual( data, cd );
				},
				( d, cd ) =>
				{
					createdhandover++;
					Assert.AreEqual( data, cd );
				},
				0 );

			e.Action();

			Assert.AreEqual( 0, blocked );
			Assert.AreEqual( 1, created );
			Assert.AreEqual( 1, started );
			Assert.AreEqual( 1, createdend );
			Assert.AreEqual( 0, createdhandover );
		}

		[TestMethod]
		public void TestCreateEventActionHandover()
		{
			int blocked = 0;
			int started = 0;
			int created = 0;
			int createdhandover = 0;
			int createdend = 0;

			var data = new CallData( 1, 5, 20, 0 );
			var station = new Station( 2, 0, 0, 10 );

			var e = new CallEvent(
				station,
				data,
				() => { blocked++; },
				() => { started++; },
				( d ) =>
				{
					created++;
					Assert.AreEqual( (uint) 0, d );
				},
				( d, cd ) =>
				{
					createdend++;
					Assert.AreEqual( data, cd );
				},
				( d, cd ) =>
				{
					createdhandover++;
					Assert.AreEqual( data, cd );
					Assert.AreEqual( (uint) 5, d );
				},
				0 );

			e.Action();

			Assert.AreEqual( 0, blocked );
			Assert.AreEqual( 1, created );
			Assert.AreEqual( 1, started );
			Assert.AreEqual( 0, createdend );
			Assert.AreEqual( 1, createdhandover );
		}
	}
}