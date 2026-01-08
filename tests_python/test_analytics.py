import unittest

from finwebanalytics import analytics


class AnalyticsTests(unittest.TestCase):
    def test_analyze_summary(self) -> None:
        result = analytics.analyze([1.0, 2.0, 3.0])
        self.assertEqual(result.summary.count, 3)
        self.assertAlmostEqual(result.summary.mean, 2.0)
        self.assertAlmostEqual(result.summary.standard_deviation, 0.8164965809, places=6)

    def test_moving_average(self) -> None:
        values = [1.0, 2.0, 3.0, 4.0]
        self.assertEqual(analytics.moving_average(values, 2), [1.5, 2.5, 3.5])

    def test_exponential_smoothing(self) -> None:
        values = [10.0, 12.0, 11.0]
        smoothed = analytics.exponential_smoothing(values, 0.5)
        self.assertEqual(len(smoothed), 3)
        self.assertGreater(smoothed[1], 10.0)


if __name__ == "__main__":
    unittest.main()
