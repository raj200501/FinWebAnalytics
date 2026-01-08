# Analytics Reference

This document describes the statistical outputs provided by FinWebAnalytics. The API focuses on
interpretable metrics that are easy to validate when integrating the service.

## Inputs

Endpoints accept a comma-separated list of numeric values via a `data` form field. Example:

```
120.5, 121.1, 119.7, 122.0, 123.4
```

Values may include whitespace. Empty values are ignored.

## Summary statistics

The `/analyze` endpoint computes:

- **Count**: number of data points supplied.
- **Minimum / Maximum**: extremal values.
- **Mean**: arithmetic average.
- **Median**: middle value (or midpoint for even-length arrays).
- **Variance**: population variance using `N` in the denominator.
- **Sample variance**: unbiased variance using `N-1` in the denominator.
- **Standard deviation**: square root of the population variance.

These values are returned in the plain-text response under the *Summary* heading and in JSON under
`summary` when using `/api/analyze`.

## Moving averages

Moving averages are calculated for windows of 3, 5, and 10 observations when enough points are
present. For example, with `values = [1, 2, 3, 4]` and window `2`, the moving average is:

```
[(1 + 2) / 2, (2 + 3) / 2, (3 + 4) / 2] = [1.5, 2.5, 3.5]
```

The text response prints one line per window. JSON responses expose the same data in the
`moving_averages` array.

## Exponential smoothing

A single smoothing series is computed with `alpha = 0.3`. The algorithm starts from the first
value and applies:

```
S_t = alpha * X_t + (1 - alpha) * S_{t-1}
```

This series smooths high-frequency noise and provides a quick look at the overall direction of the
signal.

## Trend line

The trend line is computed using a least-squares regression against the index of each value. The
result includes:

- **Slope**: directional change per step.
- **Intercept**: starting point when `x = 0`.
- **R^2**: coefficient of determination, indicating fit quality.

These values are returned at the bottom of the text response and under `trend` in the JSON
payload.

## Error handling

When parsing fails, the API responds with HTTP 400 and a descriptive error message. Examples:

- `Input data is empty. Provide comma-separated numeric values.`
- `Invalid number 'abc'`

Clients should surface these messages to help users quickly correct malformed inputs.
