from __future__ import annotations

from dataclasses import dataclass
from datetime import datetime, timezone

from finwebanalytics.parsing import FeatureInput


@dataclass(frozen=True)
class TrainingSample:
    feature1: float
    feature2: float
    target: float


@dataclass(frozen=True)
class PredictionResult:
    predicted_value: float
    model_version: str
    features: FeatureInput
    generated_at: datetime


def baseline_dataset() -> list[TrainingSample]:
    return [
        TrainingSample(0.5, 1.2, 1.6),
        TrainingSample(1.0, 0.8, 1.9),
        TrainingSample(1.5, 1.5, 2.8),
        TrainingSample(2.0, 1.7, 3.2),
        TrainingSample(2.5, 2.1, 3.9),
        TrainingSample(3.0, 2.6, 4.5),
        TrainingSample(3.5, 2.9, 5.2),
        TrainingSample(4.0, 3.1, 5.7),
        TrainingSample(4.5, 3.6, 6.4),
        TrainingSample(5.0, 3.8, 7.0),
        TrainingSample(5.5, 4.0, 7.4),
        TrainingSample(6.0, 4.2, 8.0),
        TrainingSample(6.5, 4.5, 8.7),
        TrainingSample(7.0, 4.8, 9.3),
        TrainingSample(7.5, 5.1, 10.0),
        TrainingSample(8.0, 5.5, 10.6),
        TrainingSample(8.5, 5.9, 11.3),
        TrainingSample(9.0, 6.2, 11.9),
        TrainingSample(9.5, 6.6, 12.5),
        TrainingSample(10.0, 7.0, 13.1),
        TrainingSample(10.5, 7.3, 13.7),
        TrainingSample(11.0, 7.6, 14.2),
        TrainingSample(11.5, 7.9, 14.8),
        TrainingSample(12.0, 8.2, 15.3),
        TrainingSample(12.5, 8.5, 15.9),
        TrainingSample(13.0, 8.8, 16.4),
        TrainingSample(13.5, 9.1, 17.0),
        TrainingSample(14.0, 9.4, 17.5),
        TrainingSample(14.5, 9.8, 18.1),
        TrainingSample(15.0, 10.1, 18.6),
    ]


def _solve_coefficients(samples: list[TrainingSample]) -> tuple[float, float, float]:
    n = float(len(samples))
    sum_x1 = sum(sample.feature1 for sample in samples)
    sum_x2 = sum(sample.feature2 for sample in samples)
    sum_y = sum(sample.target for sample in samples)
    sum_x1x1 = sum(sample.feature1**2 for sample in samples)
    sum_x2x2 = sum(sample.feature2**2 for sample in samples)
    sum_x1x2 = sum(sample.feature1 * sample.feature2 for sample in samples)
    sum_x1y = sum(sample.feature1 * sample.target for sample in samples)
    sum_x2y = sum(sample.feature2 * sample.target for sample in samples)

    det = (
        n * (sum_x1x1 * sum_x2x2 - sum_x1x2**2)
        - sum_x1 * (sum_x1 * sum_x2x2 - sum_x1x2 * sum_x2)
        + sum_x2 * (sum_x1 * sum_x1x2 - sum_x1x1 * sum_x2)
    )
    if det == 0:
        return 0.0, 0.0, 0.0

    det_b0 = (
        sum_y * (sum_x1x1 * sum_x2x2 - sum_x1x2**2)
        - sum_x1 * (sum_x1y * sum_x2x2 - sum_x1x2 * sum_x2y)
        + sum_x2 * (sum_x1y * sum_x1x2 - sum_x1x1 * sum_x2y)
    )
    det_b1 = (
        n * (sum_x1y * sum_x2x2 - sum_x1x2 * sum_x2y)
        - sum_y * (sum_x1 * sum_x2x2 - sum_x1x2 * sum_x2)
        + sum_x2 * (sum_x1 * sum_x2y - sum_x1y * sum_x2)
    )
    det_b2 = (
        n * (sum_x1x1 * sum_x2y - sum_x1y * sum_x1x2)
        - sum_x1 * (sum_x1 * sum_x2y - sum_x1y * sum_x2)
        + sum_y * (sum_x1 * sum_x1x2 - sum_x1x1 * sum_x2)
    )

    return det_b0 / det, det_b1 / det, det_b2 / det


def _train_model() -> tuple[float, float, float, str]:
    samples = baseline_dataset()
    intercept, coef1, coef2 = _solve_coefficients(samples)
    return intercept, coef1, coef2, "v1"


_MODEL = _train_model()


def predict(features: FeatureInput) -> PredictionResult:
    intercept, coef1, coef2, version = _MODEL
    predicted = intercept + coef1 * features.feature1 + coef2 * features.feature2
    return PredictionResult(
        predicted_value=predicted,
        model_version=version,
        features=features,
        generated_at=datetime.now(timezone.utc),
    )


def format_prediction_text(result: PredictionResult) -> str:
    return (
        "Prediction\n"
        "----------\n"
        f"Model: {result.model_version}\n"
        f"Feature1: {result.features.feature1:.4f}\n"
        f"Feature2: {result.features.feature2:.4f}\n"
        f"Predicted Value: {result.predicted_value:.4f}\n"
        f"Generated At (UTC): {result.generated_at.strftime('%Y-%m-%d %H:%M:%S')}\n"
    )
