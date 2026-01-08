# Troubleshooting

## The server will not start

- Confirm Python 3.11+ is available: `python --version`.
- Check whether the port is already in use. Set `FINWEB_PORT` to a different value and retry.
- If you see `Address already in use`, stop the conflicting process and restart the server.

## Requests return HTTP 400

The API validates input payloads. Ensure:

- The `data` field is present in the form body.
- All values are valid numbers.
- Prediction requests include at least two numbers.

Example of a valid request:

```
curl -X POST -d "data=1,2,3,4" http://localhost:5000/analyze
```

## API returns an unexpected prediction

The prediction model is deterministic. If you expect different output:

1. Confirm that `feature1` and `feature2` values are passed in the right order.
2. Ensure no locale-based formatting issues (decimal separator must be `.`).
3. Review the training data in `finwebanalytics/prediction.py` if you recently modified it.

## Smoke tests failing

`./scripts/smoke.sh` expects the service to respond to `/health`, `/analyze`, and `/predict` within
30 seconds. If failures occur:

- Inspect `/tmp/finwebanalytics.log` for server output.
- Run the server manually via `./scripts/run.sh` and retry the curl commands.
- Ensure the `curl` command is available in your environment.

## CI failures

The GitHub Actions workflow runs `./scripts/verify.sh`, which performs unit tests and smoke tests.
Re-run locally to reproduce the failure:

```
./scripts/verify.sh
```
