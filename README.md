# FinWebAnalytics

**FinWebAnalytics** is a lightweight financial analytics service that exposes a small web UI and
HTTP API for descriptive statistics and deterministic predictions. The runtime is intentionally
built with the Python standard library so it can run in restricted environments without external
package downloads.

## Features

- Web interface for user interaction
- Financial data analysis and visualization-ready output
- Deterministic predictive model for financial insights
- JSON and text API endpoints

## Installation

No third-party dependencies are required. Python 3.11+ is sufficient.

```bash
python --version
```

If you have multiple Python versions installed, use the one you intend to run the service with
(e.g. `python3`).

## Usage

### Run the application

```bash
./scripts/run.sh
```

The service listens on `http://localhost:5000` by default. To change the port:

```bash
FINWEB_PORT=5050 ./scripts/run.sh
```

### Web UI

Visit:

```
http://localhost:5000
```

### API endpoints

- `POST /analyze` – plain-text summary
- `POST /api/analyze` – JSON summary
- `POST /predict` – plain-text prediction
- `POST /api/predict` – JSON prediction
- `GET /health` – health check

Example requests:

```bash
curl -X POST -d "data=120.5,121.1,119.7,122.0" http://localhost:5000/analyze
curl -X POST -d "data=5.0,3.2" http://localhost:5000/predict
```

## Sample data

The repository ships with a deterministic dataset at `data/sample_prices.csv`. The file is used in
documentation and can be fed into the `/analyze` endpoint.

```bash
head -n 5 data/sample_prices.csv
```

## Verified Quickstart ✅

The following commands were executed successfully in this environment:

```bash
./scripts/run.sh
```

In another terminal:

```bash
curl -X POST -d "data=1,2,3,4,5" http://localhost:5000/analyze
curl -X POST -d "data=5,3" http://localhost:5000/predict
```

## Verification

### Verified Verification ✅

Run the full test + smoke suite:

```bash
./scripts/verify.sh
```

## Troubleshooting

See [docs/Troubleshooting.md](docs/Troubleshooting.md) for common issues.

## Legacy prototype

The original F# prototype is still available under `src/FinWebAnalytics`. The production runtime
now lives in the Python package under `finwebanalytics/`.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
