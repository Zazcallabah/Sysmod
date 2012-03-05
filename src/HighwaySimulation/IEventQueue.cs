namespace HighwaySimulation
{
	/// <summary>
	/// Interface for event queue
	/// </summary>
	public interface IEventQueue
	{
		/// <summary>
		/// Seed the EventQueue by adding the first element.
		/// </summary>
		void Poke();

		/// <summary>
		/// Removes the next event from the event queue and runs it.
		/// </summary>
		/// <returns>The time at which the event happened.</returns>
		uint PerformNextEvent();
	}
}