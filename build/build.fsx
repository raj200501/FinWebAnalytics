#r "paket:
nuget FSharp.Core
nuget Giraffe
nuget Expecto
nuget Microsoft.ML //"

open System
open Expecto

[<EntryPoint>]
let main _ =
    runTestsInAssembly defaultConfig
    0
