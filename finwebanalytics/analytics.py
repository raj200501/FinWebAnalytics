from __future__ import annotations

from dataclasses import dataclass
from statistics import mean, median


@dataclass(frozen=True)
class AnalysisSummary:
    count: int
    minimum: float
    maximum: float
    mean: float
    median: float
    variance: float
    sample_variance: float
    standard_deviation: float


@dataclass(frozen=True)
class MovingAverage:
    window: int
    values: list[float]


@dataclass(frozen=True)
class ExponentialSmoothing:
    alpha: float
    values: list[float]


@dataclass(frozen=True)
class TrendLine:
    slope: float
    intercept: float
    r2: float


@dataclass(frozen=True)
class AnalysisResult:
    summary: AnalysisSummary
    moving_averages: list[MovingAverage]
    exponential_smoothing: ExponentialSmoothing
    trend: TrendLine


def variance(values: list[float]) -> float:
    avg = mean(values)
    return sum((value - avg) ** 2 for value in values) / len(values)


def sample_variance(values: list[float]) -> float:
    avg = mean(values)
    return sum((value - avg) ** 2 for value in values) / (len(values) - 1)


def standard_deviation(values: list[float]) -> float:
    return variance(values) ** 0.5


def moving_average(values: list[float], window: int) -> list[float]:
    if window <= 0:
        return []

    return [mean(values[idx : idx + window]) for idx in range(len(values) - window + 1)]


def exponential_smoothing(values: list[float], alpha: float) -> list[float]:
    alpha = min(max(alpha, 0.1), 0.9)
    smoothed: list[float] = []
    last = values[0]
    for value in values:
        last = alpha * value + (1 - alpha) * last
        smoothed.append(last)
    return smoothed


def trend_line(values: list[float]) -> TrendLine:
    n = len(values)
    x_values = list(range(n))
    sum_x = sum(x_values)
    sum_y = sum(values)
    sum_xy = sum(x * y for x, y in zip(x_values, values))
    sum_x2 = sum(x * x for x in x_values)
    denominator = n * sum_x2 - sum_x**2
    if denominator == 0:
        slope = 0.0
    else:
        slope = (n * sum_xy - sum_x * sum_y) / denominator
    intercept = (sum_y - slope * sum_x) / n if n else 0.0

    predictions = [intercept + slope * x for x in x_values]
    mean_y = mean(values)
    ss_tot = sum((y - mean_y) ** 2 for y in values)
    ss_res = sum((y - y_hat) ** 2 for y, y_hat in zip(values, predictions))
    r2 = 1.0 if ss_tot == 0 else 1 - ss_res / ss_tot
    return TrendLine(slope=slope, intercept=intercept, r2=r2)


def analyze(values: list[float]) -> AnalysisResult:
    if not values:
        raise ValueError("No numeric data provided.")

    summary = AnalysisSummary(
        count=len(values),
        minimum=min(values),
        maximum=max(values),
        mean=mean(values),
        median=median(values),
        variance=variance(values),
        sample_variance=sample_variance(values) if len(values) > 1 else 0.0,
        standard_deviation=standard_deviation(values),
    )

    windows = [3, 5, 10]
    moving_averages = [
        MovingAverage(window=window, values=moving_average(values, window))
        for window in windows
        if window <= len(values)
    ]

    smoothing = ExponentialSmoothing(alpha=0.3, values=exponential_smoothing(values, 0.3))

    trend = trend_line(values)

    return AnalysisResult(
        summary=summary,
        moving_averages=moving_averages,
        exponential_smoothing=smoothing,
        trend=trend,
    )


def format_analysis_text(result: AnalysisResult) -> str:
    summary = result.summary
    moving_lines = "\n".join(
        f"Window {ma.window}: {', '.join(f'{value:.3f}' for value in ma.values)}"
        for ma in result.moving_averages
    )
    smoothing = ", ".join(f"{value:.3f}" for value in result.exponential_smoothing.values)

    return (
        "Summary\n"
        "-------\n"
        f"Count: {summary.count}\n"
        f"Minimum: {summary.minimum:.4f}\n"
        f"Maximum: {summary.maximum:.4f}\n"
        f"Mean: {summary.mean:.4f}\n"
        f"Median: {summary.median:.4f}\n"
        f"Standard Deviation: {summary.standard_deviation:.4f}\n"
        f"Variance: {summary.variance:.4f}\n"
        f"Sample Variance: {summary.sample_variance:.4f}\n\n"
        "Moving Averages\n"
        "---------------\n"
        f"{moving_lines}\n\n"
        "Exponential Smoothing (alpha=0.30)\n"
        "---------------------------------\n"
        f"{smoothing}\n\n"
        "Trend (least squares)\n"
        "---------------------\n"
        f"Slope: {result.trend.slope:.4f}\n"
        f"Intercept: {result.trend.intercept:.4f}\n"
        f"R^2: {result.trend.r2:.4f}\n"
    )
