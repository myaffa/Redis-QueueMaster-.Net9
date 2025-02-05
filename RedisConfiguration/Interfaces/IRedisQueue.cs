using RedisConfiguration.Enums;

namespace RedisConfiguration.Interfaces {
	/// <summary>
	/// Interface for sending and receiving messages from Redis queues.
	/// </summary>
	public interface IRedisQueue {
		/// <summary>
		/// Sends a message to a specified Redis queue.
		/// </summary>
		/// <param name="queueName">The name of the Redis Pub/Sub channel.</param>
		/// <param name="message">The message content to be published.</param>
		Task SendMessageAsync(EnRedisQueueName queueName, string message);

		/// <summary>
		/// Receives a message from a specified Redis queue.
		/// </summary>
		/// <param name="queueName">The name of the Redis Pub/Sub channel.</param>
		/// <returns>The received message or null if no message is received.</returns>
		Task<string?> ReceiveMessageAsync(EnRedisQueueName queueName);
	}
}
