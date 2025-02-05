using RedisConfiguration.Enums;

namespace RedisConfiguration.Interfaces {
	/// <summary>
	/// Factory interface for creating instances of Redis queues.
	/// </summary>
	public interface IRedisQueueFactory {
		/// <summary>
		/// Retrieves a Redis queue instance based on the queue type.
		/// </summary>
		IRedisQueue GetQueue(EnRedisQueueType type);
	}
}
