# FinWebAnalytics Architecture

## Overview
FinWebAnalytics is a lightweight analytics service built with the Python standard library. The
runtime deliberately avoids third-party dependencies so that the project can run in restricted
environments. The system provides a small web UI and HTTP API for running descriptive analytics
and deterministic predictions on small financial datasets.

The project is split into three major layers:

1. **Parsing layer**: validates and converts inbound request payloads into Python objects.
2. **Analytics layer**: computes summary statistics and trend analysis.
3. **Delivery layer**: exposes the HTTP endpoints and renders responses.

## Module map

| Module | Responsibility |
| --- | --- |
| `finwebanalytics/parsing.py` | Parse incoming CSV strings, validate numeric inputs, return typed values. |
| `finwebanalytics/analytics.py` | Compute statistics, moving averages, exponential smoothing, and trend lines. |
| `finwebanalytics/prediction.py` | Train and apply a deterministic linear regression model. |
| `finwebanalytics/server.py` | Serve HTML, JSON, and text endpoints. |
| `finwebanalytics/__main__.py` | CLI entry point to run the HTTP service. |
| `data/sample_prices.csv` | Sample dataset used in docs and as a reference fixture. |

## Data flow

1. **Client request** → `FinWebRequestHandler` routes the request based on method and path.
2. **Parsing** → The raw `data` form field is converted into floats. Errors return HTTP 400 with
   a descriptive message.
3. **Analytics or prediction** → The appropriate module computes the response payload.
4. **Formatting** → Results are returned as plain text for the human-friendly endpoints and
   JSON for the `/api/*` endpoints.

## Deterministic predictions

The prediction service uses a two-feature linear regression model. The coefficients are derived
from a fixed baseline dataset bundled with the application. Because the training data is fixed,
predictions are deterministic between runs, ensuring that test expectations and smoke checks
remain stable.

## HTML UI

The HTML UI is intentionally simple to avoid templating frameworks. Static assets (CSS) are served
from `finwebanalytics/static/` and the main layout lives in `finwebanalytics/templates/index.html`.
This keeps the runtime dependency-free while still providing an ergonomic user interface.

## Observability

By default the server logs only critical errors. Set `FINWEB_LOGGING=1` in the environment to
enable access logs from the built-in HTTP server.

## Configuration

- `FINWEB_PORT`: overrides the default port (5000).
- `FINWEB_LOGGING`: set to `1` to enable request logging.

## Testing strategy

The project uses `unittest` to validate:

- Parsing and validation edge cases.
- Statistical calculations (mean, variance, smoothing).
- Prediction results for consistent outputs.
- End-to-end HTTP behavior via an in-process server.

A separate shell-based smoke test validates that the deployed server responds to `/health`,
`/analyze`, and `/predict` endpoints in a live process.
