using Microsoft.Extensions.Options;
using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using RedisConfiguration.Models;
using StackExchange.Redis;
namespace RedisManagement.Messaging {
	/// <summary>
	/// Implements a Redis Stream-based queue for message handling.
	/// Uses Redis Streams to store and retrieve messages with structured entries.
	/// Implements the <see cref="IRedisQueue"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisStreamQueue(IConnectionMultiplexer redis, IOptions<RedisSettings> settings) : IRedisQueue {
		private readonly IConnectionMultiplexer _redis = redis;
		private readonly RedisSettings _settings = settings.Value;

		/// <summary>
		/// Sends a message to the Redis stream.
		/// Each message is stored as a key-value entry.
		/// </summary>
		/// <inheritdoc/>
		public async Task SendMessageAsync(EnRedisQueueName queueName, string message) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";

			// Add message to the Redis stream
			await db.StreamAddAsync(fullQueueName, [new NameValueEntry("message", message)]);
		}

		/// <summary>
		/// Reads a message from the Redis stream.
		/// This method retrieves the oldest available message in the stream.
		/// </summary>
		/// <inheritdoc/>
		public async Task<string?> ReceiveMessageAsync(EnRedisQueueName queueName) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";

			// Read the first available entry from the Redis stream
			var streamEntries = await db.StreamReadAsync(fullQueueName, "0-0", count: 1);
			if (streamEntries.Length > 0) {
				var result = streamEntries[0].Values[0].Value;
				Console.WriteLine($"Message received in RedisPubSubQueue: {result}");
				return result;
			}

			return null;
		}
	}
}
