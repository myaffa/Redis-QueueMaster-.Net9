using Microsoft.Extensions.Options;
using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using RedisConfiguration.Models;
using StackExchange.Redis;

namespace RedisManagement.Messaging {
	/// <summary>
	/// Implements a delayed queue using Redis Sorted Sets.
	/// Messages are stored with a timestamp and retrieved when they are due.
	/// Implements the <see cref="IRedisQueue"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisDelayedQueue(IConnectionMultiplexer redis, IOptions<RedisSettings> settings) : IRedisQueue {
		private readonly IConnectionMultiplexer _redis = redis;
		private readonly RedisSettings _settings = settings.Value;

		/// Adds a message to the delayed queue with the current timestamp.
		/// <inheritdoc/>
		public async Task SendMessageAsync(EnRedisQueueName queueName, string message) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";
			var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			// Store message in Redis sorted set with a timestamp as the score
			await db.SortedSetAddAsync(fullQueueName, message, timestamp);
		}

		/// Retrieves the first available message from the delayed queue.
		/// <inheritdoc/>
		public async Task<string?> ReceiveMessageAsync(EnRedisQueueName queueName) {
			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";
			// Retrieve messages that are due (timestamp is less than or equal to the current time)
			var result = await db.SortedSetRangeByScoreAsync(fullQueueName, 0, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), take: 1);

			if (result.Length > 0) {
				// Remove the retrieved message from the queue
				await db.SortedSetRemoveAsync(fullQueueName, result[0]);
				Console.WriteLine($"Message received in RedisDelayedQueue: {result[0]}");
				return result[0];
			}

			return null;
		}
	}
}
