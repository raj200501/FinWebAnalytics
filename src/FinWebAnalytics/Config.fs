namespace FinWebAnalytics

open System

module Config =
    let private getEnv name =
        match Environment.GetEnvironmentVariable(name) with
        | null | "" -> None
        | value -> Some value

    let port () =
        match getEnv "FINWEB_PORT" with
        | Some value ->
            match Int32.TryParse(value) with
            | true, parsed when parsed > 0 -> parsed
            | _ -> 5000
        | None -> 5000

    let baseUrl () =
        sprintf "http://localhost:%d" (port ())
