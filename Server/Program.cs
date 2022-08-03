using HashidsNet;
using Strona_v2.Server.Data.CommentData;
using Strona_v2.Server.Data.FileData;
using Strona_v2.Server.Data.SqlCreatTable;
using Strona_v2.Server.Data.SqlData;
using Strona_v2.Server.Data.SqlData.File;
using Strona_v2.Server.Data.TagData;
using Strona_v2.Server.EmailC;
using Strona_v2.Server.TokenAuthentication;
using Strona_v2.Shared.SqlDataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();     //głowna klasa do komunikacji z bazą danych
builder.Services.AddSingleton<ILoginUser, LoginUser>();             //log użytkownika
builder.Services.AddSingleton<IProfileUser, ProfileUser>();         //
builder.Services.AddSingleton<IEditProfileUser, EditProfileUser>(); //edycja usera
builder.Services.AddSingleton<ICreatTable, CreatTable>();            //tworzenie tabel
builder.Services.AddSingleton<ISaveFileToSQL, SaveFileToSQL>();      //zapisywanie plików
builder.Services.AddSingleton<IFileToSQL, FileToSQL>();             //polecenia sql dla plików
builder.Services.AddSingleton<ITagSql, TagSql>();                    //polecenia sql dla tag


var HashidsToken = builder.Configuration.GetValue<string>("HashidsToken");
builder.Services.AddSingleton<IHashids>(_ => new Hashids(HashidsToken, 11));// "hashowanie" id usera

builder.Services.AddSingleton<ISaveCommentToSQL, SaveCommentToSQL>();    //polecenia sql do komentarzy

//pobieranie danych z appsetting.json do serwisu od wysyłania email
var FromEmail = builder.Configuration.GetValue<string>("FluentEmail:FromEmail");
var FromName = builder.Configuration.GetValue<string>("FluentEmail:FromName");
var SmtpSenderHost = builder.Configuration.GetValue<string>("FluentEmail:SmtpSender:Host");
var SmtpSenderPort = builder.Configuration.GetValue<string>("FluentEmail:SmtpSender:Port");
var SmtpSenderUserName = builder.Configuration.GetValue<string>("FluentEmail:SmtpSender:UserName");
var SmtpSenderPassword = builder.Configuration.GetValue<string>("FluentEmail:SmtpSender:Password");
builder.Services
                .AddFluentEmail(FromEmail, FromName)
                .AddRazorRenderer()
                .AddSmtpSender(SmtpSenderHost,
                                int.Parse(SmtpSenderPort)//,
                                                         // SmtpSenderUserName,
                                                         // SmtpSenderPassword                                
                                );
builder.Services.AddScoped<IEmailSend, EmailSend>();
builder.Services.AddSingleton<IEmailVerification, EmailVerification>();
//
builder.Services.AddSingleton<ITokenManager, TokenManager>();       //autoryzacja przy użyciu tokenu

builder.Services.AddControllersWithViews().AddNewtonsoftJson();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
