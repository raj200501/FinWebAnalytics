from __future__ import annotations

import argparse

from finwebanalytics.server import run_server


def main() -> None:
    parser = argparse.ArgumentParser(description="FinWebAnalytics web server")
    parser.add_argument("--port", type=int, default=5000, help="Port to bind")
    args = parser.parse_args()
    run_server(args.port)


if __name__ == "__main__":
    main()
