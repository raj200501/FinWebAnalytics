from __future__ import annotations

import json
import os
from dataclasses import asdict
from http import HTTPStatus
from http.server import BaseHTTPRequestHandler, HTTPServer
from pathlib import Path
from typing import Callable
from urllib.parse import parse_qs

from finwebanalytics import analytics, parsing, prediction

BASE_DIR = Path(__file__).resolve().parent
STATIC_DIR = BASE_DIR / "static"
TEMPLATE_DIR = BASE_DIR / "templates"


class FinWebRequestHandler(BaseHTTPRequestHandler):
    routes: dict[str, Callable[["FinWebRequestHandler"], None]] = {}

    def do_GET(self) -> None:  # noqa: N802
        handler = self.routes.get(f"GET {self.path}")
        if handler is None:
            if self.path == "/styles.css":
                return self._serve_static("styles.css")
            return self._respond_text("Not Found", status=HTTPStatus.NOT_FOUND)
        handler(self)

    def do_POST(self) -> None:  # noqa: N802
        handler = self.routes.get(f"POST {self.path}")
        if handler is None:
            return self._respond_text("Not Found", status=HTTPStatus.NOT_FOUND)
        handler(self)

    def log_message(self, format: str, *args) -> None:  # noqa: A003
        if os.environ.get("FINWEB_LOGGING", "0") == "1":
            super().log_message(format, *args)

    def _read_form(self) -> dict[str, str]:
        length = int(self.headers.get("Content-Length", "0"))
        body = self.rfile.read(length).decode("utf-8") if length else ""
        parsed = parse_qs(body)
        return {key: value[0] for key, value in parsed.items()}

    def _respond_text(self, body: str, status: HTTPStatus = HTTPStatus.OK) -> None:
        self.send_response(status.value)
        self.send_header("Content-Type", "text/plain; charset=utf-8")
        self.end_headers()
        self.wfile.write(body.encode("utf-8"))

    def _respond_html(self, body: str, status: HTTPStatus = HTTPStatus.OK) -> None:
        self.send_response(status.value)
        self.send_header("Content-Type", "text/html; charset=utf-8")
        self.end_headers()
        self.wfile.write(body.encode("utf-8"))

    def _respond_json(self, payload: dict, status: HTTPStatus = HTTPStatus.OK) -> None:
        self.send_response(status.value)
        self.send_header("Content-Type", "application/json; charset=utf-8")
        self.end_headers()
        self.wfile.write(json.dumps(payload, indent=2, default=str).encode("utf-8"))

    def _serve_static(self, filename: str) -> None:
        file_path = STATIC_DIR / filename
        if not file_path.exists():
            return self._respond_text("Not Found", status=HTTPStatus.NOT_FOUND)
        content_type = "text/css" if filename.endswith(".css") else "application/octet-stream"
        self.send_response(HTTPStatus.OK.value)
        self.send_header("Content-Type", f"{content_type}; charset=utf-8")
        self.end_headers()
        self.wfile.write(file_path.read_bytes())


def _load_template(name: str) -> str:
    path = TEMPLATE_DIR / name
    return path.read_text(encoding="utf-8")


def handle_index(request: FinWebRequestHandler) -> None:
    request._respond_html(_load_template("index.html"))


def handle_health(request: FinWebRequestHandler) -> None:
    request._respond_text("ok")


def handle_analyze_text(request: FinWebRequestHandler) -> None:
    data = request._read_form().get("data", "")
    try:
        parsed = parsing.parse_csv_floats(data)
        result = analytics.analyze(parsed.values)
        response = analytics.format_analysis_text(result)
        request._respond_text(response)
    except ValueError as exc:
        request._respond_text(str(exc), status=HTTPStatus.BAD_REQUEST)


def handle_analyze_json(request: FinWebRequestHandler) -> None:
    data = request._read_form().get("data", "")
    try:
        parsed = parsing.parse_csv_floats(data)
        result = analytics.analyze(parsed.values)
        request._respond_json(asdict(result))
    except ValueError as exc:
        request._respond_json({"error": str(exc)}, status=HTTPStatus.BAD_REQUEST)


def handle_predict_text(request: FinWebRequestHandler) -> None:
    data = request._read_form().get("data", "")
    try:
        features = parsing.parse_two_features(data)
        result = prediction.predict(features)
        request._respond_text(prediction.format_prediction_text(result))
    except ValueError as exc:
        request._respond_text(str(exc), status=HTTPStatus.BAD_REQUEST)


def handle_predict_json(request: FinWebRequestHandler) -> None:
    data = request._read_form().get("data", "")
    try:
        features = parsing.parse_two_features(data)
        result = prediction.predict(features)
        request._respond_json(asdict(result))
    except ValueError as exc:
        request._respond_json({"error": str(exc)}, status=HTTPStatus.BAD_REQUEST)


def configure_routes() -> dict[str, Callable[[FinWebRequestHandler], None]]:
    return {
        "GET /": handle_index,
        "GET /health": handle_health,
        "POST /analyze": handle_analyze_text,
        "POST /api/analyze": handle_analyze_json,
        "POST /predict": handle_predict_text,
        "POST /api/predict": handle_predict_json,
    }


def run_server(port: int) -> None:
    handler = FinWebRequestHandler
    handler.routes = configure_routes()
    server = HTTPServer(("", port), handler)
    print(f"FinWebAnalytics listening on http://localhost:{port}")
    server.serve_forever()
