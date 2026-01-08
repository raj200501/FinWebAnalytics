import unittest

from finwebanalytics import prediction
from finwebanalytics.parsing import FeatureInput


class PredictionTests(unittest.TestCase):
    def test_prediction(self) -> None:
        features = FeatureInput(feature1=5.0, feature2=3.2)
        result = prediction.predict(features)
        self.assertGreater(result.predicted_value, 0.0)
        self.assertEqual(result.features.feature1, 5.0)
        self.assertEqual(result.features.feature2, 3.2)

    def test_baseline_dataset(self) -> None:
        dataset = prediction.baseline_dataset()
        self.assertGreaterEqual(len(dataset), 30)


if __name__ == "__main__":
    unittest.main()
