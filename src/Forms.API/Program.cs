using Asp.Versioning.ApiExplorer;
using Forms.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.RegisterServices();

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration(builder.Environment);

var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

//if (app.Environment.IsDevelopment())
//{
//app.UseDeveloperExceptionPage();

//app.UseSwaggerConfiguration(apiVersionDescriptionProvider);
//}

app.UseDeveloperExceptionPage();

app.UseSwaggerConfiguration(apiVersionDescriptionProvider);

app.UseApiConfiguration(app.Environment, builder.Configuration);

app.Run();
