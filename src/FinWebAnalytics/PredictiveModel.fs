module PredictiveModel

open Giraffe
open Microsoft.AspNetCore.Http

type Request = { data : string }

let predict (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! request = ctx.BindFormAsync<Request>()
        // Perform prediction
        let result = sprintf "Predicting based on data: %s" request.data
        return! text result next ctx
    }
