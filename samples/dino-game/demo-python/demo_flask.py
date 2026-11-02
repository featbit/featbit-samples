import atexit
import os

from fbclient import get, set_config
from fbclient.config import Config
from flask import Flask

app = Flask(__name__)

env_secret = os.getenv("FEATBIT_SAMPLE_ENV_SECRET", "")
event_url = os.getenv("FEATBIT_SAMPLE_EVENT_URL", "http://localhost:5100")
streaming_url = os.getenv("FEATBIT_SAMPLE_STREAMING_URL", "ws://localhost:5100")

set_config(Config(env_secret, event_url, streaming_url))
fb_client = get()
atexit.register(fb_client.stop)


@app.route("/demo/api/python/<user>", methods=['GET'])
def is_released_game(user: str):
    fb_user = {"key": user, "name": user}
    if fb_client.variation("runner-game", fb_user, False) is True:
        return f"Dino Game is released to {fb_user['name']}"
    else:
        return f"Dino Game not released to {fb_user['name']}"


@app.route("/demo/api/python/<user>/difficulty", methods=['GET'])
def get_difficulty_mode(user: str):
    fb_user = {"key": user, "name": user}
    if fb_client.variation("runner-game", fb_user, False) is not True:
        return f"Dino Game not released to {fb_user['name']}"
    detail = fb_client.variation_detail("difficulty-mode", fb_user, "error")
    if detail.variation_id is not None:
        return f"Dino Game is on {detail.variation} mode for {fb_user['name']}"
    return f"error: {detail.reason}"


if __name__ == "__main__":
    try:
        app.run(port=10000)
    finally:
        fb_client.stop()
