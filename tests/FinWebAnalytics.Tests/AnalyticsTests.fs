namespace FinWebAnalytics.Tests

open Expecto
open FinWebAnalytics

module AnalyticsTests =
    [<Tests>]
    let tests =
        testList "Analytics tests" [
            testCase "Median for odd sample" <| fun _ ->
                let medianValue = Analytics.median [| 1.0; 3.0; 2.0 |]
                Expect.equal medianValue 2.0 "Median should be 2"

            testCase "Moving average window" <| fun _ ->
                let values = [| 1.0; 2.0; 3.0; 4.0 |]
                let result = Analytics.movingAverage 2 values
                Expect.equal result.Length 3 "Moving average should return 3 values"
                Expect.equal result.[0] 1.5 "First moving average should be 1.5"

            testCase "Exponential smoothing" <| fun _ ->
                let values = [| 10.0; 12.0; 11.0 |]
                let smoothed = Analytics.exponentialSmoothing 0.5 values
                Expect.equal smoothed.Length 3 "Smoothing should return same length"
                Expect.isGreaterThan smoothed.[1] 10.0 "Smoothed value should move toward second point"
        ]
