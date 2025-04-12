using Microsoft.OpenApi.Models;
using VersionAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "API V2", Version = "v2" });

    // Only include actions whose GroupName matches the docName
    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.GroupName != null
            && apiDesc.GroupName.Equals(docName, StringComparison.OrdinalIgnoreCase);
    });

    options.DocumentFilter<ReplaceVersionWithExactValueInPath>();   
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "API V2");
    });

}

app.UseHttpsRedirection();


app.UseAuthorization();
app.MapControllers();

app.Run();
