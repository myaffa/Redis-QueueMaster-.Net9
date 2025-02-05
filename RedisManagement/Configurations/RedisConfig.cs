using RedisConfiguration.Configuration;
using RedisConfiguration.Interfaces;
using RedisManagement.Messaging;
using RedisManagement.Repository;

namespace RedisManagement.Configurations {
	/// <summary>
	/// Static class to configure Redis services in the application.
	/// </summary>
	public static class RedisConfig {
		/// <summary>
		/// Configures Redis services and registers message handlers.
		/// </summary>
		/// <param name="services">Service collection to register dependencies.</param>
		/// <returns>Returns the configured IServiceCollection.</returns>
		/// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
		public static IServiceCollection RedisConfiguration(this IServiceCollection services) {
			if (services == null)
				throw new ArgumentNullException(nameof(services), "Service collection cannot be null.");

			// Configure Redis settings
			services.ConfigureRedis();

			// Register Redis message handlers
			RegisterMessageHandlers(services);

			return services;
		}

		/// <summary>
		/// Registers various Redis-based queue implementations.
		/// </summary>
		/// <param name="services">Service collection to register dependencies.</param>
		private static void RegisterMessageHandlers(IServiceCollection services) {
			services.AddSingleton<RedisListQueue>();    // Standard queue
			services.AddSingleton<RedisDelayedQueue>(); // Delayed queue
			services.AddSingleton<RedisPubSubQueue>();  // Pub/Sub queue
			services.AddSingleton<RedisStreamQueue>();  // Stream-based queue

			// Register factory and lock manager
			services.AddSingleton<IRedisQueueFactory, RedisQueueFactory>();
			services.AddSingleton<IRedisLockManager, RedisLockManager>();
		}
	}
}
