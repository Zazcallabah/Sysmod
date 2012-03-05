namespace HighwaySimulation
{
	/// <summary>
	/// Base class for simulation event.
	/// </summary>
	public abstract class EventBase : IEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EventBase"/> class.
		/// </summary>
		/// <param name="triggerTime">The trigger time.</param>
		protected EventBase( uint triggerTime )
		{
			TriggerTime = triggerTime;
		}

		#region IEvent Members
		/// <summary>
		/// Gets or sets the trigger time for this event.
		/// </summary>
		/// <value></value>
		public uint TriggerTime { get; private set; }

		/// <summary>
		/// The action to run when this event triggers.
		/// </summary>
		public abstract void Action();
		#endregion
	}
}