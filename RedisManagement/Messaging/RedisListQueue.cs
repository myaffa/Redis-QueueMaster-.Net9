using Microsoft.Extensions.Options;
using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using RedisConfiguration.Models;
using StackExchange.Redis;

namespace RedisManagement.Messaging {
	/// <summary>
	/// Implements a Redis-backed list queue for message handling.
	/// Uses Redis lists to provide FIFO (First-In, First-Out) message processing.
	/// Implements the <see cref="IRedisQueue"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisListQueue(IConnectionMultiplexer redis, IOptions<RedisSettings> settings) : IRedisQueue {
		private readonly IConnectionMultiplexer _redis = redis;
		private readonly RedisSettings _settings = settings.Value;

		/// <summary>
		/// Adds a message to the Redis list queue.
		/// Messages are added to the right side of the list (FIFO behavior).
		/// </summary>
		/// <inheritdoc/>
		public async Task SendMessageAsync(EnRedisQueueName queueName, string message) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";
			Console.WriteLine($"Message sent in {fullQueueName}: {message}");
			// Push message to the right side of the Redis list (FIFO queue)
			await db.ListRightPushAsync(fullQueueName, message);  // FIFO behavior
		}

		/// <summary>
		/// Retrieves a message from the Redis list queue.
		/// Messages are removed from the left side of the list (FIFO behavior).
		/// </summary>
		/// <inheritdoc/>
		public async Task<string?> ReceiveMessageAsync(EnRedisQueueName queueName) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";
			// Pop message from the left side of the Redis list (FIFO queue)
			var result = await db.ListLeftPopAsync(fullQueueName);
			Console.WriteLine($"Message received in RedisListQueue: {result}");
			return result;
		}
	}
}
