namespace FinWebAnalytics.Tests

open Expecto
open FinWebAnalytics

module DataAnalysisTests =
    [<Tests>]
    let tests =
        testList "DataAnalysis tests" [
            testCase "Analyze data summary" <| fun _ ->
                let data = [| 1.0; 2.0; 3.0 |]
                match Analytics.analyze data with
                | Error err -> failtestf "Unexpected error: %s" err
                | Ok analysis ->
                    Expect.equal analysis.Summary.Count 3 "Count should be 3"
                    Expect.equal analysis.Summary.Mean 2.0 "Mean should be 2.0"
                    Expect.equal (System.Math.Round(analysis.Summary.StandardDeviation, 6)) 0.816497 "Standard deviation should match"
        ]
