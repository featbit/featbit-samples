### Prerequisite

Install Go 1.26.0 or later. This demo uses FeatBit Go SDK v2.1.0.

Before connecting to FeatBit with server side SDK, you should boot up the services with docker

### Environment Variables

* `FEATBIT_SAMPLE_ENV_SECRET` : this variable is mandatory. You should set your env secret to the demo app.
* `FEATBIT_SAMPLE_EVENT_URL`: remote url for insight event. The default value is `http://localhost:5100`.
* `FEATBIT_SAMPLE_STREAMING_URL`: remote url for data sync streaming The default value is `ws://localhost:5100`.
* `FEATBIT_SAMPLE_USER`: the key for the default end user; defaults to `test-user-1`.

### Run

Set the environment variables above, then run from this directory:

```sh
go mod download
go run .
```
