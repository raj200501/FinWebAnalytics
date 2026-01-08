#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
"$ROOT_DIR/scripts/bootstrap.sh"

pushd "$ROOT_DIR" >/dev/null

python -m unittest discover -s tests_python -p "test_*.py"

"$ROOT_DIR/scripts/smoke.sh"

popd >/dev/null
