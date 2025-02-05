using RedisConfiguration.Enums;
using RedisConfiguration.Interfaces;
using RedisManagement.Messaging;

namespace RedisManagement.Repository {
	/// <summary>
	/// Implements the <see cref="IRedisQueueFactory"/> interface to process messages from Redis queues.
	/// </summary>
	public class RedisQueueFactory(IServiceProvider serviceProvider) : IRedisQueueFactory {
		private readonly IServiceProvider _serviceProvider = serviceProvider;

		/// <inheritdoc/>
		public IRedisQueue GetQueue(EnRedisQueueType type) {
			return type switch {
				EnRedisQueueType.ListQueue => _serviceProvider.GetRequiredService<RedisListQueue>(),
				EnRedisQueueType.DelayedQueue => _serviceProvider.GetRequiredService<RedisDelayedQueue>(),
				EnRedisQueueType.PubSubQueue => _serviceProvider.GetRequiredService<RedisPubSubQueue>(),
				EnRedisQueueType.StreamQueue => _serviceProvider.GetRequiredService<RedisStreamQueue>(),
				_ => throw new ArgumentException("Invalid queue type")
			};
		}
	}
}
