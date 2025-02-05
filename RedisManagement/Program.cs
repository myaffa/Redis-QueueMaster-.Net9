using RedisManagement.Configurations;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add controllers and configure JSON serialization to handle enum values as strings
builder.Services.AddControllers()
	.AddJsonOptions(options => {
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

// Enable API exploration and Swagger for better API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register controllers
builder.Services.AddControllers();

// Enable OpenAPI support for better API documentation
builder.Services.AddOpenApi();

// Configure Redis services
builder.Services.RedisConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Enable HTTPS redirection for security
app.UseHttpsRedirection();

// Enable authorization middleware
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Run the application
app.Run();
