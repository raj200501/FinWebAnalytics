namespace FinWebAnalytics

open Giraffe
open Microsoft.AspNetCore.Http

module DataAnalysis =
    let private renderResult (result: AnalysisResult) =
        let summary = result.Summary
        let moving =
            result.MovingAverages
            |> List.map (fun window -> sprintf "Window %d: %s" window.Window (window.Values |> List.map (fun v -> v.ToString("0.###")) |> String.concat ", "))
            |> String.concat "\n"

        let smoothing =
            result.ExponentialSmoothing.Values
            |> List.map (fun v -> v.ToString("0.###"))
            |> String.concat ", "

        sprintf """
Summary
-------
Count: %d
Minimum: %.4f
Maximum: %.4f
Mean: %.4f
Median: %.4f
Standard Deviation: %.4f
Variance: %.4f
Sample Variance: %.4f

Moving Averages
---------------
%s

Exponential Smoothing (alpha=%.2f)
---------------------------------
%s

Trend (least squares)
---------------------
Slope: %.4f
Intercept: %.4f
R^2: %.4f
"""
            summary.Count summary.Minimum summary.Maximum summary.Mean summary.Median summary.StandardDeviation summary.Variance summary.SampleVariance
            moving
            result.ExponentialSmoothing.Alpha
            smoothing
            result.Trend.Slope result.Trend.Intercept result.Trend.R2

    let analyzeText : HttpHandler =
        fun next ctx ->
            task {
                let! request = ctx.BindFormAsync<AnalysisRequest>()
                match Parsing.parseCsvFloats request.data with
                | Error err -> return! RequestErrors.badRequest (text err) next ctx
                | Ok values ->
                    match Analytics.analyze values with
                    | Error err -> return! RequestErrors.badRequest (text err) next ctx
                    | Ok analysis ->
                        let output = renderResult analysis
                        return! text output next ctx
            }

    let analyzeJson : HttpHandler =
        fun next ctx ->
            task {
                let! request = ctx.BindFormAsync<AnalysisRequest>()
                match Parsing.parseCsvFloats request.data with
                | Error err -> return! RequestErrors.badRequest (json {| error = err |}) next ctx
                | Ok values ->
                    match Analytics.analyze values with
                    | Error err -> return! RequestErrors.badRequest (json {| error = err |}) next ctx
                    | Ok analysis -> return! json analysis next ctx
            }
