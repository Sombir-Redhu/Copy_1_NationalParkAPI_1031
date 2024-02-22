using Copy_1_NationalParkWebAPI_1031;
using Copy_1_NationalParkWebAPI_1031.Repository;
using Copy_1_NationalParkWebAPI_1031.Repository.IRepository;
using Stripe;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripSettings"));
builder.Services.AddScoped<INationalParkRepository, NationalParkRepository>();
builder.Services.AddScoped<ITrailRepository, TrailRepository>();
builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddScoped<IBookingTripRepository, BookingTripRepository>();
builder.Services.AddScoped<MessageSender>();
builder.Services.AddScoped<ISmsSender, MessageSender>();

builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("StripSettings")["SecretKey"];

app.UseAuthorization();
TwilioClient.Init(builder.Configuration.GetSection("Twilio")["AccountSid"],
              builder.Configuration.GetSection("Twilio")["AuthToken"]);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
