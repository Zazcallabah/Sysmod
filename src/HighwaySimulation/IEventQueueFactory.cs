namespace HighwaySimulation
{
	/// <summary>
	/// Interface for event queue factory
	/// </summary>
	public interface IEventQueueFactory
	{
		/// <summary>
		/// Creates the specified gatherer.
		/// </summary>
		/// <param name="gatherer">The gatherer.</param>
		/// <returns></returns>
		IEventQueue Create( IDataGatherer gatherer );
	}
}