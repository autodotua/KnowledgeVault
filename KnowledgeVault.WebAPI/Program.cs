using KnowledgeVault.WebAPI;
using KnowledgeVault.WebAPI.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

string cors = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<KnowledgeVaultDbContext>();
builder.Services.AddTransient<AchievementService>();
builder.Services.AddTransient<PropertyService>();
builder.Services.AddTransient<FileService>();
builder.Services.AddTransient<KnowledgeVaultActionFilter>();

IServiceProvider serviceProvider = null;

// Add services to the container.

builder.Services.AddControllers(o =>
{
    o.Filters.Add(serviceProvider.GetRequiredService<KnowledgeVaultActionFilter>());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(p =>
{
    var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
    var xmlPath = Path.Combine(basePath, "KnowledgeVault.WebAPI.xml");
    p.IncludeXmlComments(xmlPath);



    var scheme = new OpenApiSecurityScheme()
    {
        Description = "Authorization header",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    };
    p.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement();
    requirement[scheme] = new List<string>();
    p.AddSecurityRequirement(requirement);
});
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddCors(policy =>
{

    policy.AddPolicy(cors, opt => opt
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("X-Pagination"));

});


var app = builder.Build();
serviceProvider = app.Services;

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseSession();
app.UseCors(cors);
app.UseAuthorization();

app.MapControllers();


app.Run();
