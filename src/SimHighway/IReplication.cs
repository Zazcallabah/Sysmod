using HighwaySimulation;

namespace SimHighway
{
	/// <summary>
	/// 
	/// </summary>
	public interface IReplication
	{
		/// <summary>
		/// Gets or sets the current state of the simulation.
		/// </summary>
		ReplicationState CurrentState { get; }

		/// <summary>
		/// Gets or sets the replication data.
		/// </summary>
		/// <value>The replication data.</value>
		IDataGatherer ReplicationData { get; }

		/// <summary>
		/// Perform the simulation.
		/// </summary>
		void Run();
	}
}