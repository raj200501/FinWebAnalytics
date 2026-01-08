namespace FinWebAnalytics.Tests

open Expecto
open FinWebAnalytics

module ParsingTests =
    [<Tests>]
    let tests =
        testList "Parsing tests" [
            testCase "Parse CSV values" <| fun _ ->
                let result = Parsing.parseCsvFloats "1.0, 2.5, 3"
                match result with
                | Error err -> failtestf "Unexpected error: %s" err
                | Ok values ->
                    Expect.equal values.Length 3 "Should parse three values"
                    Expect.equal values.[1] 2.5 "Second value should be 2.5"

            testCase "Parse two features" <| fun _ ->
                match Parsing.parseTwoFeatures "4, 5" with
                | Error err -> failtestf "Unexpected error: %s" err
                | Ok features ->
                    Expect.equal features.Feature1 4.0 "Feature1 should be 4"
                    Expect.equal features.Feature2 5.0 "Feature2 should be 5"
        ]
