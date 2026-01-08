namespace FinWebAnalytics.Tests

open Expecto

module Program =
    [<EntryPoint>]
    let main argv =
        let tests =
            testList "FinWebAnalytics" [
                ParsingTests.tests
                AnalyticsTests.tests
                DataAnalysisTests.tests
                PredictiveModelTests.tests
                IntegrationTests.tests
            ]

        runTestsWithArgs defaultConfig argv tests
