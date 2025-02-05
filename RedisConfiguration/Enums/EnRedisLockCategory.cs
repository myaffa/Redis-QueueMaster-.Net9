namespace RedisConfiguration.Enums {
	/// <summary>
	/// Specifies different categories of Redis locks used in the system.
	/// </summary>
	public enum EnRedisLockCategory {
		/// <summary>
		/// Lock used for database-related operations.
		/// </summary>
		Database,

		/// <summary>
		/// Lock used for action-related operations.
		/// </summary>
		Actions
	}
}
