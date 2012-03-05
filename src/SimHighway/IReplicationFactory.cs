using HighwaySimulation;

namespace SimHighway
{
	/// <summary>
	/// 
	/// </summary>
	public interface IReplicationFactory
	{
		/// <summary>
		/// Creates a replication.
		/// </summary>
		/// <param name="gatherer">The data gatherer.</param>
		/// <param name="queueFactory">The queue factory.</param>
		/// <returns></returns>
		IReplication Create( IDataGatherer gatherer, IEventQueueFactory queueFactory );
	}
}