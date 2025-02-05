namespace RedisConfiguration.Models {
	/// <summary>
	/// Model class for storing Redis settings, which are loaded from the configuration file.
	/// </summary>
	public class RedisSettings {
		/// <summary>
		/// The hostname or IP address of the Redis server.
		/// </summary>
		public string? Host { get; set; }

		/// <summary>
		/// The port number used to connect to the Redis server.
		/// </summary>
		public int Port { get; set; }

		/// <summary>
		/// The password used for Redis authentication (if enabled).
		/// </summary>
		public string? Password { get; set; }

		/// <summary>
		/// The default database index used by Redis.
		/// </summary>
		public int Database { get; set; }

		/// <summary>
		/// A prefix for queue names to distinguish multiple environments (e.g., dev, prod).
		/// </summary>
		public string? QueuePrefix { get; set; }

		/// <summary>
		/// The maximum number of retries allowed for Redis operations.
		/// </summary>
		public int MaxRetries { get; set; }
	}
}
