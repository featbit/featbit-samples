### Prerequisite

Before connecting to FeatBit with server side SDK, you should boot up the services with docker 

### Environment Variables

* `FEATBIT_SAMPLE_ENV_SECRET` : this variable is mandatory. You should set your env secret to the demo app.
* `FEATBIT_SAMPLE_EVENT_URL`: remote url for insight event. The default value is `http://localhost:5100`.
* `FEATBIT_SAMPLE_STREAMING_URL`: remote url for data sync streaming The default value is `ws://localhost:5100`.
* `FEATBIT_SAMPLE_USER`：the key for default end user, its value is `test-user-1`. This variable is only work for Java and Python demo,
but not in SpringBoot and flask demo.

### Run the Demo
You can run the demo in your IDE after installing dependencies and setting environment variables.

If your want to run python or flask demo in command line:
* `pip install -r requirements.txt`
* set environment variables
* run the command: `python demo_python.py` or `python demo_flask.py`