using System;
using HighwaySimulation;

namespace RandomContainer
{
	/// <summary>
	/// A factory for creating Random Call Generators.
	/// </summary>
	public class RandomFactory : IRandomFactory
	{
		#region Private fields
		readonly double _callPosEnd;
		readonly double _callPosPeak;
		readonly double _callPosStart;
		readonly double _durationMean;
		readonly double _interArrivalMean;
		readonly Random _random;
		readonly double _speedDeviation;
		readonly double _speedMean;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="RandomFactory"/> class.
		/// </summary>
		/// <param name="seed">The seed.</param>
		/// <param name="callPosStart">The call pos start.</param>
		/// <param name="callPosEnd">The call pos end.</param>
		/// <param name="callPosPeak">The call pos peak.</param>
		/// <param name="speedMean">The speed mean.</param>
		/// <param name="speedDeviation">The speed deviation.</param>
		/// <param name="interArrivalMean">The inter arrival mean.</param>
		/// <param name="durationMean">The duration mean.</param>
		public RandomFactory(
			int seed,
			double callPosStart,
			double callPosEnd,
			double callPosPeak,
			double speedMean,
			double speedDeviation,
			double interArrivalMean,
			double durationMean
			)
		{
			// setup the random generator that creates the seeds used for the RandomCallGenerators (!)
			_random = new Random( seed );

			_callPosStart = callPosStart;
			_callPosEnd = callPosEnd;
			_callPosPeak = callPosPeak;
			_speedMean = speedMean;
			_speedDeviation = speedDeviation;
			_interArrivalMean = interArrivalMean;
			_durationMean = durationMean;
		}

		/// <summary>
		/// Creates a randomcallgenerator.
		/// </summary>
		/// <returns></returns>
		public IRandomCallGenerator Create()
		{
			return
				new RandomCallGenerator(
					(uint) Math.Floor( _random.NextDouble() * UInt32.MaxValue ),
					_callPosStart,
					_callPosEnd,
					_callPosPeak,
					_speedMean,
					_speedDeviation,
					_interArrivalMean,
					_durationMean );
		}
	}
}