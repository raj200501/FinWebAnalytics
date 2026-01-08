# Prediction Model

FinWebAnalytics includes a deterministic prediction model to provide consistent demo results. The
model is a two-feature linear regression trained on a bundled baseline dataset.

## Input format

The prediction endpoints expect **two numeric values** supplied as a comma-separated list:

```
5.0, 3.2
```

These values are interpreted as `feature1` and `feature2`. Additional values are ignored.

## Model training

The baseline dataset includes 30 samples with modest growth between features and targets. The
model coefficients are solved using the closed-form normal equation for two variables:

```
[ n      Σx1     Σx2 ] [ b0 ]   [ Σy   ]
[ Σx1    Σx1²    Σx1x2] [ b1 ] = [ Σx1y]
[ Σx2    Σx1x2   Σx2² ] [ b2 ]   [ Σx2y]
```

The coefficients `(b0, b1, b2)` are computed once at import time, ensuring deterministic results
for every request.

## Output fields

The prediction response includes:

- `predicted_value`: the estimated target.
- `model_version`: currently `v1`, updated when training data changes.
- `features`: echo of the inputs used.
- `generated_at`: UTC timestamp of the prediction.

Both the text and JSON endpoints include the same fields.

## Extending the model

If you want to update the model:

1. Edit `finwebanalytics/prediction.py` and update `baseline_dataset()` with new samples.
2. Update the expected values in `tests_python/test_prediction.py` if necessary.
3. Run `./scripts/verify.sh` to confirm unit tests and smoke tests still pass.
