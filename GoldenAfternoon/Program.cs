using ChelsEsite.GoldenAfternoon.Data;
using ChelsEsite.GoldenAfternoon.Resolvers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<ApplicationDbContext>(
        options => options.UseNpgsql("Host=127.0.0.1;Username=chelsesite_user;Password=chelsesite_secret"))
    .AddGraphQLServer()
    .AddDocumentFromFile("Schemas/schema.graphql")
    .BindRuntimeType<Query>()
    .BindRuntimeType<Mutation>()
    .ModifyOptions(o => o.StrictValidation = false);

var app = builder.Build();
app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args);