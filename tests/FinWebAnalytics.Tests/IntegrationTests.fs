namespace FinWebAnalytics.Tests

open System.Net
open System.Threading.Tasks
open Expecto

module IntegrationTests =
    [<Tests>]
    let tests =
        testList "Integration tests" [
            testCaseAsync "Analyze endpoint returns summary" <| fun _ ->
                task {
                    use client = TestHelpers.createClient ()
                    let! response = TestHelpers.postForm client "/analyze" "1,2,3,4"
                    let! body = response.Content.ReadAsStringAsync()
                    Expect.equal response.StatusCode HttpStatusCode.OK "Expected OK response"
                    Expect.stringContains body "Mean" "Expected mean in response"
                }

            testCaseAsync "Predict endpoint returns prediction" <| fun _ ->
                task {
                    use client = TestHelpers.createClient ()
                    let! response = TestHelpers.postForm client "/predict" "5,3"
                    let! body = response.Content.ReadAsStringAsync()
                    Expect.equal response.StatusCode HttpStatusCode.OK "Expected OK response"
                    Expect.stringContains body "Predicted Value" "Expected prediction in response"
                }

            testCaseAsync "Health endpoint ok" <| fun _ ->
                task {
                    use client = TestHelpers.createClient ()
                    let! response = client.GetAsync("/health")
                    let! body = response.Content.ReadAsStringAsync()
                    Expect.equal response.StatusCode HttpStatusCode.OK "Expected OK response"
                    Expect.equal body "ok" "Expected health response"
                }
        ]
