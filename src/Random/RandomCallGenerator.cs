using System;
using HighwaySimulation;

namespace RandomContainer
{
	/// <summary>
	/// This random call generator takes simulation data distribution parameters as arguments, and generates calls based on that data
	/// </summary>
	public class RandomCallGenerator : IRandomCallGenerator
	{
		#region Private fields
		readonly double _durationMean;
		readonly double _interArrivalMean;
		readonly RandomExtender _random;
		readonly double _speedDeviation;
		readonly double _callPosStart;
		readonly double _callPosEnd;
		readonly double _callPosPeak;
		readonly double _speedMean;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomCallGenerator"/> class.
		/// </summary>
		/// <param name="randomSeed">The random seed to use when generating data.</param>
		/// <param name="speedMean">The call travelling speed distribution mean.</param>
		/// <param name="speedDeviation">The call travelling speed distribution deviation.</param>
		/// <param name="interArrivalMean">The mean value of the inter arrival time of calls.</param>
		/// <param name="durationMean">The mean value of the call duration distribution.</param>
		/// <param name="callPosPeak">The peak, or mode, value of the call position distribution</param>
		/// <param name="callPosStart"></param>
		/// <param name="callPosEnd"></param>
		public RandomCallGenerator(
			uint randomSeed,
			double callPosStart,
			double callPosEnd,
			double callPosPeak,
			double speedMean,
			double speedDeviation,
			double interArrivalMean,
			double durationMean )
		{
			_callPosStart = callPosStart;
			_callPosEnd = callPosEnd;
			_callPosPeak = callPosPeak;
			_speedMean = speedMean;
			_speedDeviation = speedDeviation;
			_interArrivalMean = interArrivalMean;
			_durationMean = durationMean;
			_random = new RandomExtender( randomSeed );
		}

		#region IRandomCallGenerator Members
		/// <summary>
		/// Generates the random call.
		/// </summary>
		/// <param name="previousStartTime">The previous start time.</param>
		public CallData GenerateRandomCall( uint previousStartTime )
		{
			do
			{
				try
				{
					return new CallData(
						(uint) GetRandomSpeed(),
						(uint) GetRandomStartPosition(),
						(uint) GetRandomCallDuration(),
						(uint) GetRandomStartTime( previousStartTime ) );
				}
				// just in case of farout random data
				catch( ArgumentOutOfRangeException )
				{
				}
			} while( true );
		}
		#endregion

		double GetRandomStartPosition()
		{
			return _random.NextUniform()*60000 *1000;
		}

		double GetRandomSpeed()
		{
			double value = _random.NextNormal( 27.5, 3.8888889 );
			if( value < 0 || value > 1000 )
				return GetRandomSpeed();
			return value;
		}

		double GetRandomStartTime( double previousStartTime )
		{
			return previousStartTime + _random.NextExponential( 0.9703 *1000 );
		}

		double GetRandomCallDuration()
		{
			return _random.NextNormal(  201000,50000 );
		}
	}
}