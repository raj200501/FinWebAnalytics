import unittest

from finwebanalytics import parsing


class ParsingTests(unittest.TestCase):
    def test_parse_csv_floats(self) -> None:
        parsed = parsing.parse_csv_floats("1.0, 2.5, 3")
        self.assertEqual(parsed.values, [1.0, 2.5, 3.0])

    def test_parse_two_features(self) -> None:
        features = parsing.parse_two_features("4, 5")
        self.assertEqual(features.feature1, 4.0)
        self.assertEqual(features.feature2, 5.0)

    def test_parse_empty(self) -> None:
        with self.assertRaises(ValueError):
            parsing.parse_csv_floats(" ")


if __name__ == "__main__":
    unittest.main()
