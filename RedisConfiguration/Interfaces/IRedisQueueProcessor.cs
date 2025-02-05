using RedisConfiguration.Enums;

namespace RedisConfiguration.Interfaces {
	/// <summary>
	/// Interface for processing messages from Redis queues.
	/// </summary>
	public interface IRedisQueueProcessor {
		/// <summary>
		/// Processes a message from a specified Redis queue.
		/// </summary>
		/// <param name="queueName">The name of the queue from which the message is retrieved.</param>
		/// <param name="message">The message content to process.</param>
		Task ProcessMessageAsync(EnRedisQueueName queueName, string message);
	}
}
