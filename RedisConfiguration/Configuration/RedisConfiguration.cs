using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedisConfiguration.Interfaces;
using RedisConfiguration.Models;
using RedisConfiguration.Services;
using StackExchange.Redis;

namespace RedisConfiguration.Configuration {
	/// <summary>
	/// Provides an extension method for configuring Redis services within an IServiceCollection.
	/// </summary>
	public static class RedisConfiguration {
		/// <summary>
		/// Configures Redis services and adds necessary dependencies to the service collection.
		/// </summary>
		/// <param name="services">The IServiceCollection to add Redis configurations to.</param>
		/// <returns>The updated IServiceCollection with Redis services configured.</returns>
		public static IServiceCollection ConfigureRedis(this IServiceCollection services) {
			// Get the path for the Redis configuration file from the environment variable, or use the default path.
			var configPath = Environment.GetEnvironmentVariable("APP_SETTINGS_PATH")
							 ?? Path.Combine(AppContext.BaseDirectory, "Configuration", "appsettings.redis.json");

			// Build the configuration from the JSON settings file.
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Path.GetDirectoryName(configPath) ?? throw new Exception("Invalid configuration path."))
				.AddJsonFile(Path.GetFileName(configPath), optional: false, reloadOnChange: true)
				.Build();

			// Retrieve the Redis settings section from the configuration.
			var redisSettingsSection = configuration.GetSection("redis");

			// Bind the settings to a strongly-typed RedisSettings model.
			var redisSettings = new RedisSettings();
			redisSettingsSection.Bind(redisSettings);

			// Configure Redis connection options.
			var options = new ConfigurationOptions {
				EndPoints = { $"{redisSettings.Host}:{redisSettings.Port}" }, // The host and port of the Redis server.
				Password = redisSettings.Password,  // The password of the Redis server.
				DefaultDatabase = redisSettings.Database, // The default database to be used.
				AbortOnConnectFail = false, // Prevents the connection from aborting on failure.
				AllowAdmin = true, // Enables execution of admin commands.
				ConnectTimeout = 5000, // Connection timeout in milliseconds.
				SyncTimeout = 5000, // Synchronization timeout in milliseconds.
				ReconnectRetryPolicy = new ExponentialRetry(redisSettings.MaxRetries) // Exponential retry policy.
			};

			// Register the Redis connection multiplexer as a singleton.
			services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options));

			// Register Redis queue services as singletons.
			services.AddSingleton<IRedisQueueSender, RedisQueueSender>();
			services.AddSingleton<IRedisQueueProcessor, RedisQueueProcessor>();

			// Register the Redis queue listener as a hosted service.
			services.AddHostedService<RedisQueueListener>();

			return services;
		}
	}
}