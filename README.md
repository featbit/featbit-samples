# featbit-samples
This repo contains FeatBit demo for client and server side SDK:
* React
* Vue
* Java
* Python

## Server Side SDK Demo

We have pure Java, pure python, SpringBoot and Flask Demo to show how to use FeatBit SDK in your project

### Environment Variables for server side SDK

`FEATBIT_SAMPLE_ENV_SECRET` : this variable is mandatory. You should set your env secret to the demo app.
`FEATBIT_SAMPLE_EVENT_URL`: remote url for insight event. The default value is `http://localhost:5100`.
`FEATBIT_SAMPLE_STREAMING_URL`: remote url for data sync streaming The default value is `ws://localhost:5100`.
`FEATBIT_SAMPLE_USER`：the key for default end user, its value is `test-user-1`. This variable is only work for Java and Python demo,
but not in SpringBoot and flask demo.

### Run the Demo
You can run the demo in your IDE after installing dependencies and setting environment variables.

If your want to run Java or SpringBoot demo in command line:
* `mvn package`
* set environment variables
* run the command: `java -jar demo-java-1.0.1-jar-with-dependencies.jar` or `java -jar demo-springboot-1.0.1.jar`

If your want to run python or flask demo in command line:
* `pip install -r requirements.txt`
* set environment variables
* run the command: `python demo_python.py` or `python demo_flask.py`


