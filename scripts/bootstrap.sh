#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)

if ! command -v python >/dev/null 2>&1; then
  echo "Python 3.11+ is required but was not found in PATH."
  exit 1
fi

PYTHON_VERSION=$(python -c "import sys; print('.'.join(map(str, sys.version_info[:3])))")
python - <<'PY'
import sys
if sys.version_info < (3, 11):
    raise SystemExit("Python 3.11+ is required.")
PY
echo "Using python: $PYTHON_VERSION"
