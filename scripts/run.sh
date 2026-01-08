#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
"$ROOT_DIR/scripts/bootstrap.sh"

PORT=${FINWEB_PORT:-5000}

echo "Starting FinWebAnalytics on http://localhost:${PORT}"
exec python -m finwebanalytics --port "$PORT"
