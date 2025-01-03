using InventoryManagementSystem.Infrastructures.Persistence;
using InventoryManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

//DB connection
builder.Services.Configure<InventoryManagementSystemDatabaseSettings>(
    builder.Configuration.GetSection("InventoryManagementSystemDatabase"));
builder.Services.AddSingleton<ProductsService>();
builder.Services.AddSingleton<CategoriesService>();
builder.Services.AddSingleton<UsersService>();

//logger
builder.Services.AddLogging();

// Other Services
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger ve API pipeline settings
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
