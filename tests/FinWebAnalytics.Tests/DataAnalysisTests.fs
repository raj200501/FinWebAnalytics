module FinWebAnalytics.Tests.DataAnalysisTests

open Expecto
open DataAnalysis

[<Tests>]
let tests =
    testList "DataAnalysis tests" [
        testCase "Analyze data" <| fun _ ->
            let data = [| 1.0; 2.0; 3.0 |]
            let mean = data |> Array.average
            let variance = data |> Array.averageBy (fun x -> (x - mean) ** 2.0)
            let stddev = Math.Sqrt(variance)
            Expect.equal mean 2.0 "Mean should be 2.0"
            Expect.equal stddev 0.816496580927726 "Standard deviation should be 0.816496580927726"
    ]
