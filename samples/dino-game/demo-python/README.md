### Prerequisite

Use Python 3.10–3.14 and `fb-python-sdk` 1.1.10 or later (1.x).
Before connecting, start FeatBit or configure the URLs of an existing instance.
Create a boolean flag named `runner-game` and a string flag named `difficulty-mode`.

### Environment Variables

* `FEATBIT_SAMPLE_ENV_SECRET` : this variable is mandatory. You should set your env secret to the demo app.
* `FEATBIT_SAMPLE_EVENT_URL`: remote url for insight event. The default value is `http://localhost:5100`.
* `FEATBIT_SAMPLE_STREAMING_URL`: remote url for data sync streaming The default value is `ws://localhost:5100`.
* `FEATBIT_SAMPLE_USER`：the key for default end user, its value is `test-user-1`. This variable is only work for Java and Python demo,
but not in SpringBoot and flask demo.

### Run the Demo
You can run the demo in your IDE after installing dependencies and setting environment variables.

If your want to run python or flask demo in command line:
* `python -m pip install --upgrade -r requirements.txt`
* set environment variables
* run the command: `python demo_python.py` or `python demo_flask.py`

The Flask demo serves `/demo/api/python/<user>` and
`/demo/api/python/<user>/difficulty` on port 10000.

Both demos use `variation("runner-game", user, False)` and read
`variation_detail()` through its `variation`, `reason`, and `variation_id`
properties. A missing variation ID indicates an evaluation fallback. The SDK is
closed on normal shutdown to give pending events a chance to be delivered.

To try changes from a local SDK checkout, install it after the dependencies:

```shell
python -m pip install -e ../../../../featbit-python-sdk
```

This path assumes `featbit-samples` and `featbit-python-sdk` are sibling directories
and the command runs from this demo directory. Local installation is required to
try unpublished changes such as the new `insightsEnabled` handling.
