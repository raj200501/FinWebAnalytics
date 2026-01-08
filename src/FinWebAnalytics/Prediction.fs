namespace FinWebAnalytics

open System

module Prediction =
    type TrainingSample = {
        Feature1: float
        Feature2: float
        Target: float
    }

    type Model = {
        Intercept: float
        Coef1: float
        Coef2: float
        Version: string
    }

    let private trainingData =
        [
            { Feature1 = 0.5; Feature2 = 1.2; Target = 1.6 }
            { Feature1 = 1.0; Feature2 = 0.8; Target = 1.9 }
            { Feature1 = 1.5; Feature2 = 1.5; Target = 2.8 }
            { Feature1 = 2.0; Feature2 = 1.7; Target = 3.2 }
            { Feature1 = 2.5; Feature2 = 2.1; Target = 3.9 }
            { Feature1 = 3.0; Feature2 = 2.6; Target = 4.5 }
            { Feature1 = 3.5; Feature2 = 2.9; Target = 5.2 }
            { Feature1 = 4.0; Feature2 = 3.1; Target = 5.7 }
            { Feature1 = 4.5; Feature2 = 3.6; Target = 6.4 }
            { Feature1 = 5.0; Feature2 = 3.8; Target = 7.0 }
            { Feature1 = 5.5; Feature2 = 4.0; Target = 7.4 }
            { Feature1 = 6.0; Feature2 = 4.2; Target = 8.0 }
            { Feature1 = 6.5; Feature2 = 4.5; Target = 8.7 }
            { Feature1 = 7.0; Feature2 = 4.8; Target = 9.3 }
            { Feature1 = 7.5; Feature2 = 5.1; Target = 10.0 }
            { Feature1 = 8.0; Feature2 = 5.5; Target = 10.6 }
            { Feature1 = 8.5; Feature2 = 5.9; Target = 11.3 }
            { Feature1 = 9.0; Feature2 = 6.2; Target = 11.9 }
            { Feature1 = 9.5; Feature2 = 6.6; Target = 12.5 }
            { Feature1 = 10.0; Feature2 = 7.0; Target = 13.1 }
            { Feature1 = 10.5; Feature2 = 7.3; Target = 13.7 }
            { Feature1 = 11.0; Feature2 = 7.6; Target = 14.2 }
            { Feature1 = 11.5; Feature2 = 7.9; Target = 14.8 }
            { Feature1 = 12.0; Feature2 = 8.2; Target = 15.3 }
            { Feature1 = 12.5; Feature2 = 8.5; Target = 15.9 }
            { Feature1 = 13.0; Feature2 = 8.8; Target = 16.4 }
            { Feature1 = 13.5; Feature2 = 9.1; Target = 17.0 }
            { Feature1 = 14.0; Feature2 = 9.4; Target = 17.5 }
            { Feature1 = 14.5; Feature2 = 9.8; Target = 18.1 }
            { Feature1 = 15.0; Feature2 = 10.1; Target = 18.6 }
        ]

    let private solveCoefficients (samples: TrainingSample list) =
        let n = float samples.Length
        let sumX1 = samples |> List.sumBy (fun s -> s.Feature1)
        let sumX2 = samples |> List.sumBy (fun s -> s.Feature2)
        let sumY = samples |> List.sumBy (fun s -> s.Target)
        let sumX1X1 = samples |> List.sumBy (fun s -> s.Feature1 * s.Feature1)
        let sumX2X2 = samples |> List.sumBy (fun s -> s.Feature2 * s.Feature2)
        let sumX1X2 = samples |> List.sumBy (fun s -> s.Feature1 * s.Feature2)
        let sumX1Y = samples |> List.sumBy (fun s -> s.Feature1 * s.Target)
        let sumX2Y = samples |> List.sumBy (fun s -> s.Feature2 * s.Target)

        // Solve the normal equation for two features + intercept.
        // Matrix form:
        // [ n      sumX1    sumX2 ] [ b0 ]   [ sumY   ]
        // [ sumX1  sumX1X1  sumX1X2] [ b1 ] = [ sumX1Y]
        // [ sumX2  sumX1X2  sumX2X2] [ b2 ]   [ sumX2Y]
        let det =
            n * (sumX1X1 * sumX2X2 - sumX1X2 * sumX1X2)
            - sumX1 * (sumX1 * sumX2X2 - sumX1X2 * sumX2)
            + sumX2 * (sumX1 * sumX1X2 - sumX1X1 * sumX2)

        if det = 0.0 then
            { Intercept = 0.0; Coef1 = 0.0; Coef2 = 0.0; Version = "fallback" }
        else
            let detB0 =
                sumY * (sumX1X1 * sumX2X2 - sumX1X2 * sumX1X2)
                - sumX1 * (sumX1Y * sumX2X2 - sumX1X2 * sumX2Y)
                + sumX2 * (sumX1Y * sumX1X2 - sumX1X1 * sumX2Y)
            let detB1 =
                n * (sumX1Y * sumX2X2 - sumX1X2 * sumX2Y)
                - sumY * (sumX1 * sumX2X2 - sumX1X2 * sumX2)
                + sumX2 * (sumX1 * sumX2Y - sumX1Y * sumX2)
            let detB2 =
                n * (sumX1X1 * sumX2Y - sumX1Y * sumX1X2)
                - sumX1 * (sumX1 * sumX2Y - sumX1Y * sumX2)
                + sumY * (sumX1 * sumX1X2 - sumX1X1 * sumX2)

            {
                Intercept = detB0 / det
                Coef1 = detB1 / det
                Coef2 = detB2 / det
                Version = "v1"
            }

    let private model = solveCoefficients trainingData

    let predict (input: PredictionInput) =
        let predicted = model.Intercept + model.Coef1 * input.Feature1 + model.Coef2 * input.Feature2
        {
            PredictedValue = predicted
            ModelVersion = model.Version
            Features = input
            GeneratedAt = DateTimeOffset.UtcNow
        }

    let baselineDataset () = trainingData
