namespace FinWebAnalytics.Tests

open System.Collections.Generic
open System.Net.Http
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.Hosting
open FinWebAnalytics

module TestHelpers =
    let createClient () =
        let hostBuilder =
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(fun webHostBuilder ->
                    webHostBuilder
                        .UseTestServer()
                        .Configure(Program.configureApp)
                        .ConfigureServices(Program.configureServices)
                    |> ignore)

        let host = hostBuilder.Start()
        host.GetTestClient()

    let postForm (client: HttpClient) endpoint data =
        let content = new FormUrlEncodedContent([ KeyValuePair("data", data) ])
        client.PostAsync(endpoint, content)
