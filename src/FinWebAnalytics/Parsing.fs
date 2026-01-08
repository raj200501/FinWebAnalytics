namespace FinWebAnalytics

open System

module Parsing =
    let private toFloat (value: string) =
        let trimmed = value.Trim()
        match Double.TryParse(trimmed) with
        | true, number -> Ok number
        | false, _ -> Error (sprintf "Invalid number '%s'" trimmed)

    let parseCsvFloats (raw: string) =
        if String.IsNullOrWhiteSpace(raw) then
            Error "Input data is empty. Provide comma-separated numeric values."
        else
            let parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
            if parts.Length = 0 then
                Error "No numeric values found in input data."
            else
                parts
                |> Array.map toFloat
                |> Array.fold (fun acc item ->
                    match acc, item with
                    | Error err, _ -> Error err
                    | _, Error err -> Error err
                    | Ok values, Ok number -> Ok (number :: values)
                ) (Ok [])
                |> Result.map (fun values -> values |> List.rev |> List.toArray)

    let parseTwoFeatures (raw: string) =
        match parseCsvFloats raw with
        | Error err -> Error err
        | Ok values when values.Length < 2 ->
            Error "Provide at least two numeric values to run a prediction."
        | Ok values ->
            Ok {
                Feature1 = values.[0]
                Feature2 = values.[1]
            }
