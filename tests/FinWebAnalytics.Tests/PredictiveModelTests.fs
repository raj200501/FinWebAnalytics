namespace FinWebAnalytics.Tests

open Expecto
open FinWebAnalytics

module PredictiveModelTests =
    [<Tests>]
    let tests =
        testList "PredictiveModel tests" [
            testCase "Predict deterministic value" <| fun _ ->
                let input = { PredictionInput.Feature1 = 5.0; Feature2 = 3.2 }
                let prediction = Prediction.predict input
                Expect.isGreaterThan prediction.PredictedValue 0.0 "Predicted value should be positive"
                Expect.equal prediction.Features.Feature1 5.0 "Feature1 should be echoed"
                Expect.equal prediction.Features.Feature2 3.2 "Feature2 should be echoed"
        ]
