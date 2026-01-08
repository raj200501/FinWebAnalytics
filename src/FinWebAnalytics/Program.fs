namespace FinWebAnalytics

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe

module Program =
    let configureApp (app : IApplicationBuilder) =
        app.UseStaticFiles() |> ignore
        app.UseGiraffe WebApp.webApp

    let configureServices (services : IServiceCollection) =
        services.AddGiraffe() |> ignore

    [<EntryPoint>]
    let main _ =
        let url = Config.baseUrl()
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(fun webHostBuilder ->
                webHostBuilder
                    .Configure(configureApp)
                    .ConfigureServices(configureServices)
                    .UseUrls(url)
                    |> ignore)
            .Build()
            .Run()
        0
