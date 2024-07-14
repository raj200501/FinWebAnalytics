module DataAnalysis

open System
open Giraffe
open Microsoft.AspNetCore.Http

type Request = { data : string }

let analyze (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! request = ctx.BindFormAsync<Request>()
        let data = request.data.Split(',') |> Array.map float
        // Perform data analysis (e.g., calculating mean and standard deviation)
        let mean = data |> Array.average
        let variance = data |> Array.averageBy (fun x -> (x - mean) ** 2.0)
        let stddev = Math.Sqrt(variance)
        let result = sprintf "Data: %A\nMean: %f\nStandard Deviation: %f" data mean stddev
        return! text result next ctx
    }
