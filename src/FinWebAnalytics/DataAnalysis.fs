module DataAnalysis

open Giraffe
open Microsoft.AspNetCore.Http

type Request = { data : string }

let analyze (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! request = ctx.BindFormAsync<Request>()
        // Perform data analysis
        let result = sprintf "Analyzing data: %s" request.data
        return! text result next ctx
    }
