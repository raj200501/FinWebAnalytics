module FinWebAnalytics.Tests.PredictiveModelTests

open Expecto
open PredictiveModel

[<Tests>]
let tests =
    testList "PredictiveModel tests" [
        testCase "Predict data" <| fun _ ->
            let data = { Feature1 = 1.0f; Feature2 = 2.0f }
            let prediction = predict data
            Expect.isTrue (prediction.PredictedValue >= 0.0f) "Predicted value should be non-negative"
    ]
