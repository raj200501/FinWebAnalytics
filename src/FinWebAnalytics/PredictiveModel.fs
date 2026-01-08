namespace FinWebAnalytics

open Giraffe
open Microsoft.AspNetCore.Http

module PredictiveModel =
    let private renderText (result: PredictionResult) =
        sprintf """
Prediction
----------
Model: %s
Feature1: %.4f
Feature2: %.4f
Predicted Value: %.4f
Generated At (UTC): %s
""" result.ModelVersion result.Features.Feature1 result.Features.Feature2 result.PredictedValue (result.GeneratedAt.ToString("u"))

    let predictText : HttpHandler =
        fun next ctx ->
            task {
                let! request = ctx.BindFormAsync<PredictionRequest>()
                match Parsing.parseTwoFeatures request.data with
                | Error err -> return! RequestErrors.badRequest (text err) next ctx
                | Ok features ->
                    let prediction = Prediction.predict features
                    let output = renderText prediction
                    return! text output next ctx
            }

    let predictJson : HttpHandler =
        fun next ctx ->
            task {
                let! request = ctx.BindFormAsync<PredictionRequest>()
                match Parsing.parseTwoFeatures request.data with
                | Error err -> return! RequestErrors.badRequest (json {| error = err |}) next ctx
                | Ok features ->
                    let prediction = Prediction.predict features
                    return! json prediction next ctx
            }
