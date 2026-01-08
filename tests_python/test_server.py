import threading
import time
import unittest
from http.client import HTTPConnection

from finwebanalytics.server import FinWebRequestHandler, configure_routes
from http.server import HTTPServer


class ServerTests(unittest.TestCase):
    def setUp(self) -> None:
        FinWebRequestHandler.routes = configure_routes()
        self.server = HTTPServer(("", 0), FinWebRequestHandler)
        self.thread = threading.Thread(target=self.server.serve_forever)
        self.thread.daemon = True
        self.thread.start()
        self.port = self.server.server_address[1]
        time.sleep(0.1)

    def tearDown(self) -> None:
        self.server.shutdown()
        self.thread.join(timeout=2)
        self.server.server_close()

    def _request(self, method: str, path: str, body: str | None = None) -> tuple[int, str]:
        conn = HTTPConnection("localhost", self.port, timeout=5)
        headers = {}
        if body is not None:
            headers["Content-Type"] = "application/x-www-form-urlencoded"
        conn.request(method, path, body=body, headers=headers)
        response = conn.getresponse()
        payload = response.read().decode("utf-8")
        conn.close()
        return response.status, payload

    def test_health_endpoint(self) -> None:
        status, body = self._request("GET", "/health")
        self.assertEqual(status, 200)
        self.assertEqual(body, "ok")

    def test_predict_endpoint(self) -> None:
        status, body = self._request("POST", "/predict", body="data=5,3")
        self.assertEqual(status, 200)
        self.assertIn("Predicted Value", body)


if __name__ == "__main__":
    unittest.main()
