open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open WebApp

let webApp = choose [
    route "/" >=> htmlView Views.index
    route "/analyze" >=> POST >=> bindForm<DataAnalysis.Request> DataAnalysis.analyze
    route "/predict" >=> POST >=> bindForm<PredictiveModel.Request> PredictiveModel.predict
]

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffe webApp

let configureServices (services : IServiceCollection) =
    services.AddGiraffe() |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(configureApp)
                .ConfigureServices(configureServices)
                .UseUrls("http://localhost:5000")
                |> ignore)
        .Build()
        .Run()
    0
