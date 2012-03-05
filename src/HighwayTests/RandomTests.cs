using System;
using System.Collections.Generic;
using System.Linq;
using HighwaySimulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RandomContainer;

namespace HighwayTests
{
	/// <summary>
	/// </summary>
	[TestClass]
	public class RandomTests
	{
		[TestMethod]
		public void GenerateTriangularAndTest()
		{
			double start = 0, end = 20, peak = 5;
			var r = new RandomExtender( 1044400 );
			uint ranges = 8;
			uint samples = 10000;
			IList<Range> dist = GetDistributionForFunction( samples, () => r.NextTriangular( start, end, peak ), start, end, ranges );

			double certainty = 0.9;
			var expectedranges = new double[ranges];
			double rangewidth = ( end - start ) / ranges;
			// fill to end
			for( int i = 0; i < ranges; i++ )
				expectedranges[i] = ( samples
					* IntegrateTriang( start + ( i * rangewidth ), start + ( ( i + 1 ) * rangewidth ), start, end, peak ) );

			for( int i = 0; i < dist.Count; i++ )
				Assert.IsTrue(
					dist[i].HitCount >= expectedranges[i] * certainty && dist[i].HitCount <= expectedranges[i] * ( 2 - certainty ) );
		}

		[TestMethod]
		public void TestMersenneAlgorithIsNotBiased()
		{
			var r = new MersenneTwister( 65530035 );
			uint ranges = 2;
			uint samples = 10000;

			uint rangea = 0, rangeb = 0;

			for( uint i = 0; i < samples; i++ )
				if( r.Next( 2 ) == 0 )
					rangea++;
				else
					rangeb++;

			double certainty = 0.98;
			uint expectedall = samples / ranges;

			Assert.IsTrue( rangea > expectedall * certainty && rangea < expectedall * ( 2 - certainty ) );
			Assert.IsTrue( rangeb > expectedall * certainty && rangeb < expectedall * ( 2 - certainty ) );
		}

		[TestMethod]
		public void BenchmarkMersenne()
		{
			ulong samples = 100000;
			int runs = 2;

			// first try System.Random, for baseline
			var baseline = GetAverage<int>( runs, samples, s => new Random( s ).Next );
			var baselinei = GetAverage<int, int>( runs, samples, ( s ) => new Random( s ).Next, Int32.MaxValue );
			var baselineii = GetAverage<int, int, int>( runs, samples, ( s ) => new Random( s ).Next, 8, Int32.MaxValue );

			//matching mt functions:
			var mersenne = GetAverage<int>( runs, samples, s => new MersenneTwister( (uint) s ).Next );
			var mersennei = GetAverage<int, int>( runs, samples, ( s ) => new MersenneTwister( (uint) s ).Next, Int32.MaxValue );
			var mersenneii = GetAverage<int, int, int>( runs, samples, ( s ) => new MersenneTwister( (uint) s ).Next, 8, Int32.MaxValue );

			// some other interesting mt functions
			var mersenneu = GetAverage<uint>( runs, samples, s => new MersenneTwister( (uint) s ).NextUInt );
			var mersenneuu = GetAverage<uint, uint>( runs, samples, ( s ) => new MersenneTwister( (uint) s ).NextUInt, UInt32.MaxValue );
			var mersenneuuu = GetAverage<uint, uint, uint>( runs, samples, ( s ) => new MersenneTwister( (uint) s ).NextUInt, 8, UInt32.MaxValue );

		}

		TimeSpan GetAverage<TRet>( int runs, ulong samples, Func<int, Func<TRet>> createRandom )
		{
			Random r = new Random();
			long totalticks = 0;
			for( int i = 0; i < runs; i++ )
			{
				totalticks += GetTicksForFunction( samples, createRandom( r.Next() ) );

			}
			return new TimeSpan( totalticks / runs );
		}
		TimeSpan GetAverage<TOne, TRet>( int runs, ulong samples, Func<int, Func<TOne, TRet>> createRandom, TOne one )
		{
			Random r = new Random();
			long totalticks = 0;
			for( int i = 0; i < runs; i++ )
			{
				totalticks += GetTicksForFunction( samples, createRandom( r.Next() ), one );

			}
			return new TimeSpan( totalticks / runs );
		}
		TimeSpan GetAverage<TOne, TTwo, TRet>( int runs, ulong samples, Func<int, Func<TOne, TTwo, TRet>> createRandom, TOne one, TTwo two )
		{
			Random r = new Random();
			long totalticks = 0;
			for( int i = 0; i < runs; i++ )
			{
				totalticks += GetTicksForFunction( samples, createRandom( r.Next() ), one, two );

			}
			return new TimeSpan( totalticks / runs );
		}
		long GetTicksForFunction<TRet>( ulong samples, Func<TRet> getRandom )
		{
			long before = DateTime.Now.Ticks;
			for( ulong i = 0; i < samples; i++ )
			{
				getRandom();
			}
			long after = DateTime.Now.Ticks;

			return after - before;
		}
		long GetTicksForFunction<TOne, TRet>( ulong samples, Func<TOne, TRet> getRandom, TOne one )
		{
			long before = DateTime.Now.Ticks;
			for( ulong i = 0; i < samples; i++ )
			{
				getRandom( one );
			}
			long after = DateTime.Now.Ticks;

			return after - before;
		}
		long GetTicksForFunction<TOne, TTwo, TRet>( ulong samples, Func<TOne, TTwo, TRet> getRandom, TOne one, TTwo two )
		{
			long before = DateTime.Now.Ticks;
			for( ulong i = 0; i < samples; i++ )
			{
				getRandom( one, two );
			}
			long after = DateTime.Now.Ticks;

			return after - before;
		}

		[TestMethod]
		public void RandomFactoryCreatesSameSequencOfGeneratorsForSameSeed()
		{
			int seed = 42;

			var f0 = new RandomFactory( seed, 10, 2, 1, 10, 1, 1, 1 );
			IRandomCallGenerator r01 = f0.Create();
			IRandomCallGenerator r02 = f0.Create();

			var f1 = new RandomFactory( seed, 10, 2, 1, 10, 1, 1, 1 );
			IRandomCallGenerator r11 = f1.Create();
			IRandomCallGenerator r12 = f1.Create();

			Assert.AreEqual( r11.GenerateRandomCall( 0 ), r01.GenerateRandomCall( 0 ) );
			Assert.AreEqual( r11.GenerateRandomCall( 50 ), r01.GenerateRandomCall( 50 ) );
			Assert.AreNotEqual( r11.GenerateRandomCall( 1120 ), r01.GenerateRandomCall( 2203 ) );
			Assert.AreEqual( r11.GenerateRandomCall( 55 ), r01.GenerateRandomCall( 55 ) );

			Assert.AreEqual( r02.GenerateRandomCall( 0 ), r12.GenerateRandomCall( 0 ) );
			Assert.AreEqual( r02.GenerateRandomCall( 10 ), r12.GenerateRandomCall( 10 ) );
			Assert.AreEqual( r02.GenerateRandomCall( 0 ), r12.GenerateRandomCall( 0 ) );
			Assert.AreEqual( r02.GenerateRandomCall( 10 ), r12.GenerateRandomCall( 10 ) );

			Assert.AreNotEqual( r11.GenerateRandomCall( 0 ), r12.GenerateRandomCall( 0 ) );
		}

		[TestMethod]
		public void RandomExtenderDistributionAlgorithmUniformTests()
		{
			var r = new RandomExtender( 1000 );
			uint ranges = 5;
			uint samples = 10000;
			IList<Range> dist = GetDistributionForFunction( samples, r.NextUniform, 0, 1, ranges );

			double certainty = 0.9;
			uint expectedall = samples / ranges;

			foreach( Range range in dist )
				Assert.IsTrue( range.HitCount > expectedall * certainty && range.HitCount < expectedall * ( 2 - certainty ) );
		}

		[TestMethod]
		public void RandomExtenderDistributionAlgorithmNormalTests()
		{
			var r = new RandomExtender( 1000 );
			double mean = 0;
			double deviation = 1;
			uint ranges = 10;
			uint samples = 1000;
			double min = -2.5;
			double max = 2.5;
			IList<Range> dist = GetDistributionForFunction( samples, () => r.NextNormal( mean, deviation ), min, max, ranges );

			double certainty = 0.8;

			var expectedranges = new double[ranges];
			double rangewidth = ( max - min ) / ranges;
			for( int i = 0; i < ranges; i++ )
				expectedranges[i] = ( samples
					* IntegrateNormal( min + ( i * rangewidth ), min + ( ( i + 1 ) * rangewidth ), mean, deviation ) );

			for( int i = 0; i < dist.Count; i++ )
				Assert.IsTrue(
					dist[i].HitCount >= expectedranges[i] * certainty && dist[i].HitCount <= expectedranges[i] * ( 2 - certainty ) );
		}

		IList<Range> GetDistributionForFunction( uint samplesize, Func<double> algorithm, double algorithmMinVal, double algorithMaxVal, uint rangecount )
		{
			var doubles = new double[samplesize];
			for( int i = 0; i < doubles.Length; i++ )
				doubles[i] = algorithm();

			var ranges = new List<Range>( (int) rangecount );
			double rangespan = ( algorithMaxVal - algorithmMinVal ) / rangecount;

			for( int i = 0; i < rangecount; i++ )
				ranges.Add( new Range( algorithmMinVal + i * rangespan, rangespan ) );

			ranges.Add( Range.Realm );
			foreach( double d in doubles )
				ranges.First( r => r.IsInRange( d ) ).HitCount++;
			ranges.Remove( ranges.Last() );
			return ranges;
		}

		#region TriangularHelpers
		double IntegrateTriang( double start, double end, double min, double max, double peak )
		{
			return CDFT( end, min, max, peak ) - CDFT( start, min, max, peak );
		}

		double CDFT( double x, double min, double max, double peak )
		{
			if( x > peak )
				return 1.0 - Math.Pow( ( max - x ), 2 ) / ( ( max - min ) * ( max - peak ) );
			return Math.Pow( ( x - min ), 2 ) / ( ( max - min ) * ( peak - min ) );
		}
		#endregion

		#region Normalhelpers
		double IntegrateNormal( double start, double end, double mean, double deviation )
		{
			return CDF( end, mean, deviation ) - CDF( start, mean, deviation );
		}

		double CDF( double x, double mean, double deviation )
		{
			double value = ( 1.0 / 2 ) + ( ( 1.0 / 2 ) * ERF( ( x - mean ) / ( deviation * Math.Sqrt( 2 ) ), 30 ) );
			if( value < 0 )
				return 0;
			if( value > 1 )
				return 1;
			return value;
		}

		double ERF( double x, ulong ordo )
		{
			double returnvalue = 0;
			for( ulong n = 0; n < ordo; n++ )
				returnvalue += ( ( Math.Pow( -1, n ) * Math.Pow( x, ( 2 * n ) + 1 ) ) / ( Fac( n ) * ( ( 2 * n ) + 1 ) ) );
			return returnvalue * ( 2 / Math.Sqrt( Math.PI ) );
		}

		ulong Fac( ulong x )
		{
			ulong ret = 1;
			while( x > 1 )
				ret *= x--;
			return ret;
		}
		#endregion

		#region Nested type: Range
		class Range
		{
			#region Private fields
			double _end;
			double _start;
			#endregion

			public Range( double start, double length )
			{
				_start = start;
				_end = start + length;
			}

			public static Range Realm
			{
				get
				{
					var r = new Range( 0, 0 );
					r._end = Double.MaxValue;
					r._start = Double.MinValue;
					return r;
				}
			}

			public uint HitCount { get; set; }

			public override bool Equals( object obj )
			{
				if( ReferenceEquals( null, obj ) )
					return false;
				if( ReferenceEquals( this, obj ) )
					return true;
				if( obj.GetType() != typeof( Range ) )
					return false;
				return Equals( (Range) obj );
			}

			public bool IsInRange( double x )
			{
				return x > _start && x <= _end;
			}

			public bool Equals( Range other )
			{
				if( ReferenceEquals( null, other ) )
					return false;
				if( ReferenceEquals( this, other ) )
					return true;
				return other._end == _end && other._start == _start && other.HitCount == HitCount;
			}

			public override int GetHashCode()
			{
				unchecked
				{
					int result = _end.GetHashCode();
					result = ( result * 397 ) ^ _start.GetHashCode();
					result = ( result * 397 ) ^ HitCount.GetHashCode();
					return result;
				}
			}

			public override string ToString()
			{
				return string.Format( "{0}->{1}: {2}", _start, _end, HitCount );
			}

		}
		#endregion
	}
}