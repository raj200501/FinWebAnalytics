namespace FinWebAnalytics

open System

[<CLIMutable>]
type AnalysisRequest = {
    data: string
}

[<CLIMutable>]
type PredictionRequest = {
    data: string
}

type AnalysisSummary = {
    Count: int
    Minimum: float
    Maximum: float
    Mean: float
    Median: float
    StandardDeviation: float
    Variance: float
    SampleVariance: float
}

type MovingAverage = {
    Window: int
    Values: float list
}

type ExponentialSmoothing = {
    Alpha: float
    Values: float list
}

type TrendLine = {
    Slope: float
    Intercept: float
    R2: float
}

type AnalysisResult = {
    Summary: AnalysisSummary
    MovingAverages: MovingAverage list
    ExponentialSmoothing: ExponentialSmoothing
    Trend: TrendLine
}

type PredictionInput = {
    Feature1: float
    Feature2: float
}

type PredictionResult = {
    PredictedValue: float
    ModelVersion: string
    Features: PredictionInput
    GeneratedAt: DateTimeOffset
}
