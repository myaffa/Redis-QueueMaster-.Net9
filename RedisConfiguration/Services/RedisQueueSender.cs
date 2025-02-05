using RedisConfiguration.Interfaces;
using StackExchange.Redis;
using Microsoft.Extensions.Options;
using RedisConfiguration.Models;
using RedisConfiguration.Enums;

namespace RedisConfiguration.Services {
	/// <summary>
	/// Implements the <see cref="IRedisQueueSender"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisQueueSender(IConnectionMultiplexer redis, IOptions<RedisSettings> settings) : IRedisQueueSender {
		private readonly IConnectionMultiplexer _redis = redis ?? throw new ArgumentNullException(nameof(redis));
		private readonly RedisSettings _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));

		/// <inheritdoc/>
		public async Task SendToQueueAsync(EnRedisQueueName queueName, string message) {
			if (string.IsNullOrWhiteSpace(message))
				throw new ArgumentNullException(nameof(message), "Message cannot be null or empty.");

			var db = _redis.GetDatabase();
			var fullQueueName = $"{_settings.QueuePrefix}{queueName}";  // Add the prefix to the queue name

			await db.ListRightPushAsync(fullQueueName, message);
			Console.WriteLine($"✅ Message sent to Redis queue: {queueName}");
		}
	}
}
