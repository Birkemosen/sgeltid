using JasperFx.Core;
using Marten;
using Marten.Exceptions;
using Npgsql;
using Oakton;
using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.Marten;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ApplyOaktonExtensions();

#region Wolverine_Event_Subscription_Config

builder.Host.UseWolverine(opts =>
{
    // If we encounter a concurrency exception, just try it immediately
    // up to 3 times total
    opts.Policies.OnException<ConcurrencyException>().RetryTimes(3);

    // It's an imperfect world, and sometimes transient connectivity errors
    // to the database happen
    opts.Policies.OnException<NpgsqlException>()
        .RetryWithCooldown(50.Milliseconds(), 100.Milliseconds(), 250.Milliseconds());

    // Automatic usage of transactional middleware as
    // Wolverine recognizes that an HTTP endpoint or message handler
    // persists data
    opts.Policies.AutoApplyTransactions();

    opts.Policies.UseDurableLocalQueues();
});

#endregion

builder.Services.AddModules();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Wolverine_Opting_Into_Event_Publishing

builder.Services.AddMarten(opts =>
    {
        opts.Connection(builder.Configuration.GetConnectionString("marten"));
        opts.DatabaseSchemaName = "sgeltid";
    })
    // Added this to enroll Marten in the Wolverine outbox
    .IntegrateWithWolverine()

    // Added this to opt into events being forwarded to the 
    // Wolverine outbox during SaveChangesAsync()
    .EventForwardingToWolverine();

#endregion

var app = builder.Build();
app.AddEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// This is using the Oakton library for command running
await app.RunOaktonCommands(args);  //app.Run();