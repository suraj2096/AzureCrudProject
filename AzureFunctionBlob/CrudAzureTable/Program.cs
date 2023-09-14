using Azure;
using CrudAzureTable.Hubs;
using CrudAzureTable.Models;
using CrudAzureTable.Repository;
using CrudAzureTable.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITableStorageRepository<FileData>, TableStorageReoository<FileData>>();
builder.Services.AddScoped<IAuthenticateRepository,AuthenticateRepository>();
builder.Services.AddSignalR(options => { options.KeepAliveInterval = TimeSpan.FromSeconds(5); });



builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapHub<ListenHub>("/getDataSignal");





app.MapGet("/getAllData/{id}", async (int id,ITableStorageRepository<FileData> tableStorageRepository) =>
{
    return Results.Ok(await tableStorageRepository.GetAllEntityAsync(id));
});

app.MapGet("/getData", async (string partititonKey,string rowKey,ITableStorageRepository<FileData> tableStorageRepository) =>
{
    var data = await tableStorageRepository.GetEntityAsync(partititonKey,rowKey);
    if (data == null) return Results.BadRequest();
    return Results.Ok(data);

});


app.MapPost("/createData", async (FileData data, ITableStorageRepository<FileData> tableStorageRepository) =>
{
    return Results.Ok(await tableStorageRepository.CreateEntityAsync(data));
});


app.MapPut("/UpdateData", async (FileData data, ITableStorageRepository<FileData> tableStorageRepository) =>
{
    var getMessage = await tableStorageRepository.UpsertEntityAsync(data);
    if(getMessage) return Results.Ok(new {Staus=1, Message = "Updated Successfully" });
    return Results.BadRequest(new { Staus = 0, Message = "Somehting went wrong" });

});

app.MapDelete("/DeleteData/{partititonKey}/{rowKey}", async (string partititonKey, string rowKey, ITableStorageRepository<FileData> tableStorageRepository) =>
{
    var getMessage = await tableStorageRepository.DeleteEntityAsync(partititonKey,rowKey);
    if (getMessage) return Results.Ok(new { Staus = 1, Message = "Deleted Successfully" });
    return Results.BadRequest(new { Staus = 0, Message = "Somehting went wrong" });

});



// login user here 
app.MapGet("/login/{userName}/{password}", (string userName, string password,IAuthenticateRepository iautenticateRepository) =>
{
     var data = iautenticateRepository.authenticateUser(userName, password);
    if (data != null) return Results.Ok(new {Status=1, Message = "login successfully",data=new{userName=data.UserName,Id=data.RowKey  } });
    return Results.BadRequest(new { Status = 0, Message = "login unsuccessfully" });
});



app.UseCors("MyPolicy");





app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}