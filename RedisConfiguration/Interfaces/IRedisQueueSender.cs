using RedisConfiguration.Enums;

namespace RedisConfiguration.Interfaces {
	/// <summary>
	/// Interface for sending messages to Redis queues.
	/// </summary>
	public interface IRedisQueueSender {
		/// <summary>
		/// Sends a message to a specified Redis queue.
		/// </summary>
		/// <param name="queueName">The target queue where the message should be sent.</param>
		/// <param name="message">The message to be added to the queue.</param>
		Task SendToQueueAsync(EnRedisQueueName queueName, string message);
	}
}
