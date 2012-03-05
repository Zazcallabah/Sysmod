using HighwaySimulation;

namespace SimHighway
{
	/// <summary>
	/// Factory for creating Replication objects
	/// </summary>
	public class ReplicationFactory : IReplicationFactory
	{
		#region Private fields
		readonly uint _replicationLength;
		readonly uint _startup;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="ReplicationFactory"/> class.
		/// </summary>
		/// <param name="replicationLength">Length of the replication.</param>
		/// <param name="startup">The startup.</param>
		public ReplicationFactory( uint replicationLength, uint startup )
		{
			_replicationLength = replicationLength;
			_startup = startup;
		}

		/// <summary>
		/// Creates a replication.
		/// </summary>
		/// <param name="gatherer">The data gatherer.</param>
		/// <param name="queueFactory">The queue factory.</param>
		/// <returns></returns>
		public IReplication Create( IDataGatherer gatherer, IEventQueueFactory queueFactory )
		{
			return new Replication( _replicationLength, gatherer, queueFactory, _startup );
		}
	}
}