module PredictiveModel

open System
open Giraffe
open Microsoft.AspNetCore.Http
open Microsoft.ML
open Microsoft.ML.Data

type FinancialData = { Feature1: float32; Feature2: float32 }
type Prediction = { PredictedValue: float32 }

type Request = { data : string }

let predict (next : HttpFunc) (ctx : HttpContext) =
    task {
        let! request = ctx.BindFormAsync<Request>()
        let data = request.data.Split(',') |> Array.map float32
        if data.Length < 2 then
            return! text "Insufficient data for prediction" next ctx
        else
            let mlContext = MLContext()
            let sampleData = { Feature1 = data.[0]; Feature2 = data.[1] }

            // Load the model and predict
            let modelPath = "model.zip"
            let loadedModel = mlContext.Model.Load(modelPath, out _)
            let predictionEngine = mlContext.Model.CreatePredictionEngine<FinancialData, Prediction>(loadedModel)
            let prediction = predictionEngine.Predict(sampleData)

            let result = sprintf "Predicted Value: %f" prediction.PredictedValue
            return! text result next ctx
    }
