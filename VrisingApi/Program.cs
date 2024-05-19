using VrisingApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConfig, Configuration>();
builder.Services.AddScoped<IVrisingLog, VrisingLog>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/vrisingserver", (IVrisingLog serverLog) => serverLog.GetServerData())
    .WithName("GetVrisingServerData")
    .WithOpenApi();

app.Run();
