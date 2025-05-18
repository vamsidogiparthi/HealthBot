using HealthCareAgent.WebAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddOpenApi();
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddLogging(p =>
    p.AddConsole().AddConfiguration(builder.Configuration.GetSection("Logging"))
);
builder.Services.AddOptions();
builder.Services.AddSingleton<IChatHistoryDataService, ChatHistoryDataService>();
builder.Services.Configure<OpenAIConfiguration>(
    configuration.GetSection(OpenAIConfiguration.SectionName)
);

builder.Services.Configure<ChatHistoryDataServiceConfiguration>(
    configuration.GetSection(ChatHistoryDataServiceConfiguration.SectionName)
);

builder.Services.Configure<GoogleAIConfiguration>(
    configuration.GetSection(GoogleAIConfiguration.SectionName)
);

builder.Services.AddSingleton<IChatCompletionService>(sp =>
{
    var openAIConfiguration =
        configuration.GetSection(OpenAIConfiguration.SectionName).Get<OpenAIConfiguration>()
        ?? throw new Exception("OpenAI configuration is missing");

    var googleAIConfiguration =
        configuration.GetSection(GoogleAIConfiguration.SectionName).Get<GoogleAIConfiguration>()
        ?? throw new Exception("Google AI configuration is missing");

#pragma warning disable SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    // return new GoogleAIGeminiChatCompletionService(
    //     googleAIConfiguration.Model,
    //     googleAIConfiguration.ApiKey
    // );
#pragma warning restore SKEXP0070 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    return new OpenAIChatCompletionService(openAIConfiguration.Model, openAIConfiguration.ApiKey);
});

builder.Services.AddSingleton<HealthCareAgent.Brain.Plugins.FunctionPlugins.TimePlugin>();
builder.Services.AddSingleton<UserIntentPlugin>();
builder.Services.AddSingleton<MedicalProviderDatabasePlugin>();
builder.Services.AddSingleton<SummaryPlugin>();
builder.Services.AddSingleton<ConversationSummaryPlugin>();

builder.Services.AddKeyedTransient(
    "ChatBotKernel",
    (sp, key) =>
    {
        KernelPluginCollection kernelFunctions = [];
        kernelFunctions.AddFromObject(
            sp.GetRequiredService<HealthCareAgent.Brain.Plugins.FunctionPlugins.TimePlugin>()
        );
        kernelFunctions.AddFromObject(sp.GetRequiredService<UserIntentPlugin>());
        kernelFunctions.AddFromObject(sp.GetRequiredService<MedicalProviderDatabasePlugin>());
        kernelFunctions.AddFromObject(sp.GetRequiredService<SummaryPlugin>());
        kernelFunctions.AddFromObject(sp.GetRequiredService<ConversationSummaryPlugin>());
        return new Kernel(sp, kernelFunctions);
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.MapControllers();
app.MapHub<ChatHub>("/ChatHub");
app.UseHttpsRedirection();
