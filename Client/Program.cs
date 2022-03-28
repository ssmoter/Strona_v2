using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Strona_v2.Client;
using Strona_v2.Client.Data;
using Strona_v2.Client.Data.API;
using Strona_v2.Client.Data.File.Dowloand;
using Strona_v2.Client.Data.File.Upload;
using Strona_v2.Client.Data.Toast;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage(); //local storage
builder.Services.AddAuthorizationCore();    //autoryzacja
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();     //autoryzacja moja modyfikacja
builder.Services.AddScoped<IApiUser, ApiUser>();        //klasa z api user
builder.Services.AddScoped<IApiUserWithToken, ApiUserWithToken>(); //klasa z api oraz tokenem 
builder.Services.AddScoped<ToastService>();                  //wyskakujące okienka z informacjami
builder.Services.AddScoped<BrowserService>();                //wielkość okna w przeglądarce chyba nie przydatne
builder.Services.AddScoped<IAPIFileDowloand, APIFileDowloand>(); //pobieranie nowego posta
builder.Services.AddScoped<IUploadFileApi, UploadFileApi>();     //Wysyłnie nowego posta


await builder.Build().RunAsync();
