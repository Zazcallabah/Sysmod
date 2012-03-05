namespace HighwaySimulation
{
	/// <summary>
	/// Factory for creating event queues
	/// </summary>
	public class EventQueueFactory : IEventQueueFactory
	{
		#region Private fields
		readonly uint _channels;
		readonly IRandomCallGenerator _generator;
		readonly uint _highwayLength;
		readonly uint _reservedChannels;
		readonly uint _stationCount;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="EventQueueFactory"/> class.
		/// </summary>
		/// <param name="generator">The generator.</param>
		/// <param name="stationCount">The station count.</param>
		/// <param name="highwayLength">Length of the highway.</param>
		/// <param name="channels">The channels.</param>
		/// <param name="reservedChannels">The reserved channels.</param>
		public EventQueueFactory(
			IRandomCallGenerator generator,
			uint stationCount,
			uint highwayLength,
			uint channels,
			uint reservedChannels )
		{
			_generator = generator;
			_stationCount = stationCount;
			_highwayLength = highwayLength;
			_channels = channels;
			_reservedChannels = reservedChannels;
		}

		/// <summary>
		/// Creates the specified gatherer.
		/// </summary>
		/// <param name="gatherer">The gatherer.</param>
		/// <returns></returns>
		public IEventQueue Create( IDataGatherer gatherer )
		{
			return new EventQueue(
				gatherer,
				_generator,
				_stationCount,
				_highwayLength,
				_channels,
				_reservedChannels );
		}
	}
}