namespace SimHighway
{
	/// <summary>
	/// Represents the state the simulation is in
	/// </summary>
	public enum ReplicationState
	{
		/// <summary>
		/// Simulation hasn't started yet
		/// </summary>
		NotStarted,

		/// <summary>
		/// Simulation is in warm up state, no data is being recorded.
		/// </summary>
		WarmUp,

		/// <summary>
		/// Simulation is running
		/// </summary>
		Running,

		/// <summary>
		/// Not implemented
		/// </summary>
		Paused,

		/// <summary>
		/// Simulation has finished.
		/// </summary>
		Finished,

		/// <summary>
		/// Simulation has crashed. Badness.
		/// </summary>
		Crashed
	} ;
}