namespace RedisConfiguration.Enums {
	/// <summary>
	/// Defines the different types of Redis queues used in the system.
	/// </summary>
	public enum EnRedisQueueType {
		/// <summary>
		/// Represents a list-based Redis queue.
		/// </summary>
		ListQueue,

		/// <summary>
		/// Represents a delayed queue where messages are processed after a delay.
		/// </summary>
		DelayedQueue,

		/// <summary>
		/// Represents a Redis Pub/Sub queue used for message broadcasting.
		/// </summary>
		PubSubQueue,

		/// <summary>
		/// Represents a Redis Stream queue for handling event-driven data flows.
		/// </summary>
		StreamQueue
	}
}
