namespace FinWebAnalytics

open Giraffe

module WebApp =
    let webApp =
        choose [
            GET >=> choose [
                route "/" >=> htmlView Views.index
                route "/health" >=> text "ok"
            ]
            POST >=> choose [
                route "/analyze" >=> DataAnalysis.analyzeText
                route "/api/analyze" >=> DataAnalysis.analyzeJson
                route "/predict" >=> PredictiveModel.predictText
                route "/api/predict" >=> PredictiveModel.predictJson
            ]
        ]
