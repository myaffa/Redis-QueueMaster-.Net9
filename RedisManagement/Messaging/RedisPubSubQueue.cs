using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using StackExchange.Redis;

namespace RedisManagement.Messaging {
	/// <summary>
	/// Implements a Redis Publish-Subscribe (Pub/Sub) queue.
	/// Messages are published to a channel, and subscribers receive them in real-time.
	/// Implements the <see cref="IRedisQueue"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisPubSubQueue(IConnectionMultiplexer redis) : IRedisQueue {
		private readonly IConnectionMultiplexer _redis = redis;

		/// <summary>
		/// Publishes a message to a specified Redis Pub/Sub channel.
		/// </summary>
		/// <inheritdoc/>
		public async Task SendMessageAsync(EnRedisQueueName queueName, string message) {
			var pubSub = _redis.GetSubscriber();
			// Publish the message to the specified Redis channel
			await pubSub.PublishAsync(RedisChannel.Literal(queueName.ToString()), message);
		}

		/// <summary>
		/// Subscribes to a Redis Pub/Sub channel and listens for messages.
		/// Note: This method currently returns only the latest received message.
		/// </summary>
		/// <inheritdoc/>
		public async Task<string?> ReceiveMessageAsync(EnRedisQueueName queueName) {
			var pubSub = _redis.GetSubscriber();
			string? receivedMessage = null;

			// Subscribe to the specified Redis channel
			await pubSub.SubscribeAsync(RedisChannel.Literal(queueName.ToString()), (channel, message) => {
				receivedMessage = message;
			});
			Console.WriteLine($"Message received in RedisPubSubQueue: {receivedMessage}");
			return receivedMessage;
		}
	}
}
