using FHotel.Repository.Infrastructures;
using FHotel.Repository.Models;
using FHotel.Service.Profiles;
using FHotel.Service.Services.Implementations;
using FHotel.Service.Services.Interfaces;
using FHotel.Services.Services.Implementations;
using FHotel.Services.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
builder.Services.AddControllers();
// Read the connection string from appsettings.json
string connectionString = configuration.GetConnectionString("MyCnn");

// Add DbContext using the connection string
builder.Services.AddDbContext<FHotelContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Add other services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelAmenityService, HotelAmenityService>();
builder.Services.AddScoped<IHotelRegistrationService, HotelRegistrationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentMethodService, PaymentMethodService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IReservationDetailService, ReservationDetailService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomFacilityService, RoomFacilityService>();
builder.Services.AddScoped<IRoomImageService, RoomImageService>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IRoomTypePriceService, RoomTypePriceService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceTypeService, ServiceTypeService>();
builder.Services.AddScoped<ITimetableService, TimetableService>();
builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IBillOrderService, BillOrderService>();
builder.Services.AddScoped<IBillLateCheckOutChargeService, BillLateCheckOutChargeService>();
builder.Services.AddScoped<IBillPaymentService, BillPaymentService>();
builder.Services.AddScoped<ILateCheckOutChargeService, LateCheckOutChargeService>();
builder.Services.AddScoped<ILateCheckOutPolicyService, LateCheckOutPolicyService>();
builder.Services.AddScoped<IRefundService, RefundService>();
builder.Services.AddScoped<IRefundPolicyService, RefundPolicyService>();
builder.Services.AddScoped<IRoomStayHistoryService, RoomStayHistoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IWalletHistoryService, WalletHistoryService>();



//mapper
builder.Services.AddAutoMapper(typeof(ApplicationMapper));

//khai app AppSettings: Secret Key
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MeowLish API",
        Description = "MeowLish Management API",
        TermsOfService = new Uri("https://FHotel.com"),
        Contact = new OpenApiContact
        {
            Name = "MeowLish Company",
            Email = "meowlish.work@gmail.com",
            Url = new Uri("https://twitter.com/FHotel"),
        },
        License = new OpenApiLicense
        {
            Name = "MeowLish Open License",
            Url = new Uri("https://FHotel.com"),
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Token with Bearer format"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{ }
    }
});

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
