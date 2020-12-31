# Utilities
Codes which I use most frequently across different project.

## Utilities.Data
#### DataAccess Helper which can be used with Any DataBase.

To use this, you can register in Startup ConfigureServices method for DI.
``` C#
services.AddSingleton<ISqlHelper>(x => ActivatorUtilities.CreateInstance<SqlHelper>(x, "Microsoft.Data.SqlClient", connectionString);
```


## Utilities.Api
#### ApiHelper (HttpClient) helper. It used IHttpClientFactory. [How to make HTTP requests](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0 "How to use IHttpClientFactory and HttpClient to make the Http call.")

To use this Helper register in Startup.cs ConfigureServices method for DI.
``` C#
services.AddHttpClient(); // Unnamed HttpClient.
services.AddHttpClient("MyApi" client =>
{
    client.BaseAddress = new System.Uri("https://api.mywebsite.com"),
    client.DefaultRequestHeaders.Add("Accept", "application/json") // Not necessary
});
services.AddSingleton<IApiHelper, ApiHelper>();
```
