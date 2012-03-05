namespace HighwaySimulation
{
	/// <summary>
	/// Interface for simulated Events
	/// </summary>
	public interface IEvent
	{
		/// <summary>
		/// Gets or sets the trigger time for this event.
		/// </summary>
		uint TriggerTime { get; }

		/// <summary>
		/// The action to run when this event triggers.
		/// </summary>
		void Action();
	}
}