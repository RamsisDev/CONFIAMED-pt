using Supabase;
using UsuariosService.Repositories;

var builder = WebApplication.CreateBuilder(args);
string? supaUrl =
    Environment.GetEnvironmentVariable("SUPABASE_URL") ??
    builder.Configuration["Supabase:Url"] ??
    builder.Configuration["SupabaseUrl"];

string? supaKey =
    Environment.GetEnvironmentVariable("SUPABASE_KEY") ??
    builder.Configuration["Supabase:Key"] ??
    builder.Configuration["SupabaseKey"];

if (string.IsNullOrWhiteSpace(supaUrl) || string.IsNullOrWhiteSpace(supaKey))
    throw new InvalidOperationException("Falta SUPABASE_URL/SUPABASE_KEY o Supabase:Url/Supabase:Key");


var supaOptions = new SupabaseOptions { AutoConnectRealtime = true, AutoRefreshToken = true };
builder.Services.AddSingleton(_ => new Supabase.Client(supaUrl!, supaKey!, supaOptions));

builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();

//peticiones del front
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalAngular",
        p => p.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              );
});

// Permitir peticiones desde Tareas
builder.Services.AddCors(options =>
{
    options.AddPolicy("MSTareas",
        p => p.WithOrigins("http://localhost:5057")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              );
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var log = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    var supabase = scope.ServiceProvider.GetRequiredService<Supabase.Client>();
    try
    {
        await supabase.InitializeAsync();

        var rest = (Supabase.Postgrest.Client)supabase.Postgrest;

        // Opción A (preferida): usar Options.Schema si está disponible en tu versión
        if (rest.Options != null)
        {
            rest.Options.Schema = "confiamed_tarea_system";
        }
        else
        {
            var prev = rest.GetHeaders;
            rest.GetHeaders = () =>
            {
                var h = prev?.Invoke() ?? new Dictionary<string, string>();
                h["Accept-Profile"] = "confiamed_tarea_system";
                h["Content-Profile"] = "confiamed_tarea_system";
                return h;
            };
        }

        log.LogInformation("✅ Supabase inicializado. URL={Url}", supaUrl);
    }
    catch (Exception ex)
    {
        log.LogError(ex, "❌ Error inicializando Supabase");
    }
}


app.UseCors("LocalAngular");
app.UseCors("MSTareas");

//if (app.Environment.IsDevelopment())
//{
//    app.UseHttpsRedirection();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
