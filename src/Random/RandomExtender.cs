using System;

namespace RandomContainer
{
	/// <summary>
	/// Implementation of IRandomExtender that uses a Mersenne Twister algorithm to generate random numbers.
	/// </summary>
	public class RandomExtender : IRandomExtender
	{
		#region Private fields
		readonly Random _random;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomExtender"/> class.
		/// </summary>
		/// <param name="seed">The seed.</param>
		public RandomExtender( uint seed )
		{
			_random = new MersenneTwister( seed );
		}

		/// <summary>
		/// Get a uniformly distributed value betveen 0(inclusive) and 1(exclusive)
		/// </summary>
		/// <returns>The random value.</returns>
		public double NextUniform()
		{
			return _random.NextDouble();
		}

		/// <summary>
		/// Get a normal distributed value  centered around the parameter mean and with the given deviation
		/// </summary>
		/// <param name="mean">The mean value of the distribution</param>
		/// <param name="deviation">The deviation of the distribution</param>
		/// <returns>The random value.</returns>
		public double NextNormal( double mean, double deviation )
		{
			// algorithm from ftp://ftp.taygeta.com/pub/c/boxmuller.c
			double x1, w;

			do
			{
				x1 = ( 2.0 * _random.NextDouble() ) - 1.0;
				double x2 = ( 2.0 * _random.NextDouble() ) - 1.0;
				w = ( x1 * x1 ) + ( x2 * x2 );
			} while( w >= 1.0 );

			w = Math.Sqrt( ( -2.0 * Math.Log( w ) ) / w );

			return mean + ( x1 * w * deviation );
		}

		/// <summary>
		/// Get an exponentially distrubuted value with parameter mean as  1/lambda
		/// </summary>
		/// <param name="mean">The mean value of the distribution</param>
		/// <returns>The random value.</returns>
		public double NextExponential( double mean )
		{
			return -mean * Math.Log( _random.NextDouble() );
		}

		/// <summary>
		/// Get a triangularly distributed value with set parameters.
		/// </summary>
		/// <param name="start">The start point of the distribution.</param>
		/// <param name="end">The end point of the destribution.</param>
		/// <param name="peak">The peak point of the distribution.</param>
		/// <returns></returns>
		public double NextTriangular( double start, double end, double peak )
		{
			double randomValue = _random.NextDouble();

			if( randomValue <= ( peak - start ) / ( end - start ) )
				randomValue = start + Math.Sqrt( randomValue * ( end - start ) * ( peak - start ) );
			else
				randomValue = end - Math.Sqrt( ( 1 - randomValue ) * ( end - start ) * ( end - peak ) );
			return randomValue;
		}
	}
}