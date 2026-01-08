import csv
import pathlib
import unittest


class DatasetTests(unittest.TestCase):
    def test_sample_dataset(self) -> None:
        path = pathlib.Path("data/sample_prices.csv")
        self.assertTrue(path.exists())
        with path.open(newline="", encoding="utf-8") as handle:
            reader = csv.DictReader(handle)
            rows = list(reader)
        self.assertGreaterEqual(len(rows), 1500)
        self.assertIn("price", rows[0])


if __name__ == "__main__":
    unittest.main()
