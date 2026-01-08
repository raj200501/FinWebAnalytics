#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
"$ROOT_DIR/scripts/bootstrap.sh"

PORT=${FINWEB_PORT:-5055}
BASE_URL="http://localhost:${PORT}"

cleanup() {
  if [[ -n "${APP_PID:-}" ]]; then
    kill "$APP_PID" >/dev/null 2>&1 || true
  fi
}
trap cleanup EXIT

python -m finwebanalytics --port "$PORT" >/tmp/finwebanalytics.log 2>&1 &
APP_PID=$!

for _ in {1..30}; do
  if curl -fsS "$BASE_URL/health" >/dev/null 2>&1; then
    break
  fi
  sleep 1
  if ! kill -0 "$APP_PID" >/dev/null 2>&1; then
    echo "Application failed to start. Logs:"
    cat /tmp/finwebanalytics.log
    exit 1
  fi
  if [[ $_ -eq 30 ]]; then
    echo "Application did not become healthy in time."
    exit 1
  fi
 done

ANALYZE_RESPONSE=$(curl -fsS -X POST -d "data=1,2,3,4,5" "$BASE_URL/analyze")
if [[ "$ANALYZE_RESPONSE" != *"Mean"* ]]; then
  echo "Analyze response missing mean"
  echo "$ANALYZE_RESPONSE"
  exit 1
fi

PREDICT_RESPONSE=$(curl -fsS -X POST -d "data=5,3" "$BASE_URL/predict")
if [[ "$PREDICT_RESPONSE" != *"Predicted Value"* ]]; then
  echo "Predict response missing predicted value"
  echo "$PREDICT_RESPONSE"
  exit 1
fi

echo "Smoke test passed."
