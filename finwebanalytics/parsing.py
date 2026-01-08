from __future__ import annotations

from dataclasses import dataclass


@dataclass(frozen=True)
class ParsedData:
    values: list[float]


@dataclass(frozen=True)
class FeatureInput:
    feature1: float
    feature2: float


def parse_csv_floats(raw: str) -> ParsedData:
    if raw is None or not raw.strip():
        raise ValueError("Input data is empty. Provide comma-separated numeric values.")

    parts = [part.strip() for part in raw.split(",") if part.strip()]
    if not parts:
        raise ValueError("No numeric values found in input data.")

    values: list[float] = []
    for part in parts:
        try:
            values.append(float(part))
        except ValueError as exc:
            raise ValueError(f"Invalid number '{part}'") from exc

    return ParsedData(values=values)


def parse_two_features(raw: str) -> FeatureInput:
    parsed = parse_csv_floats(raw)
    if len(parsed.values) < 2:
        raise ValueError("Provide at least two numeric values to run a prediction.")

    return FeatureInput(feature1=parsed.values[0], feature2=parsed.values[1])
