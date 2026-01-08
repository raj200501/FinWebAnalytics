namespace FinWebAnalytics

open System

module Analytics =
    let private ensureNonEmpty (values: float array) =
        if values.Length = 0 then
            Error "No numeric data provided."
        else
            Ok values

    let mean values =
        values |> Array.average

    let variance values =
        let avg = mean values
        values |> Array.averageBy (fun x -> (x - avg) ** 2.0)

    let sampleVariance values =
        let avg = mean values
        let numerator = values |> Array.sumBy (fun x -> (x - avg) ** 2.0)
        numerator / float (values.Length - 1)

    let stddev values =
        sqrt (variance values)

    let median values =
        let sorted = values |> Array.sort
        let middle = sorted.Length / 2
        if sorted.Length % 2 = 0 then
            (sorted.[middle - 1] + sorted.[middle]) / 2.0
        else
            sorted.[middle]

    let min values = values |> Array.min
    let max values = values |> Array.max

    let movingAverage window values =
        if window <= 0 then
            []
        else
            values
            |> Array.windowed window
            |> Array.map mean
            |> Array.toList

    let exponentialSmoothing alpha values =
        let alpha' =
            if alpha <= 0.0 then 0.1
            elif alpha >= 1.0 then 0.9
            else alpha

        let smoothed =
            values
            |> Array.fold (fun (acc, last) value ->
                let nextValue = alpha' * value + (1.0 - alpha') * last
                (nextValue :: acc, nextValue)
            ) ([], values.[0])
            |> fst
            |> List.rev

        smoothed

    let private linearRegression (values: float array) =
        let n = float values.Length
        let x = [| 0.0 .. float (values.Length - 1) |]
        let sumX = x |> Array.sum
        let sumY = values |> Array.sum
        let sumXY = Array.map2 (fun xi yi -> xi * yi) x values |> Array.sum
        let sumX2 = x |> Array.sumBy (fun xi -> xi * xi)
        let denominator = (n * sumX2) - (sumX * sumX)
        let slope =
            if denominator = 0.0 then 0.0
            else ((n * sumXY) - (sumX * sumY)) / denominator
        let intercept =
            if n = 0.0 then 0.0
            else (sumY - slope * sumX) / n
        let predictions = x |> Array.map (fun xi -> intercept + slope * xi)
        let ssTot = values |> Array.sumBy (fun yi -> (yi - mean values) ** 2.0)
        let ssRes = Array.map2 (fun yi yhat -> (yi - yhat) ** 2.0) values predictions |> Array.sum
        let r2 = if ssTot = 0.0 then 1.0 else 1.0 - (ssRes / ssTot)
        { Slope = slope; Intercept = intercept; R2 = r2 }

    let analyze (values: float array) : Result<AnalysisResult, string> =
        ensureNonEmpty values
        |> Result.map (fun data ->
            let summary = {
                Count = data.Length
                Minimum = min data
                Maximum = max data
                Mean = mean data
                Median = median data
                StandardDeviation = stddev data
                Variance = variance data
                SampleVariance = if data.Length > 1 then sampleVariance data else 0.0
            }

            let moving =
                [ 3; 5; 10 ]
                |> List.filter (fun window -> window <= data.Length)
                |> List.map (fun window ->
                    {
                        Window = window
                        Values = movingAverage window data
                    })

            let smoothing = {
                Alpha = 0.3
                Values = exponentialSmoothing 0.3 data
            }

            {
                Summary = summary
                MovingAverages = moving
                ExponentialSmoothing = smoothing
                Trend = linearRegression data
            })
