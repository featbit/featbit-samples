import os
from time import sleep
from fbclient import set_config, get
from fbclient.config import Config

env_secret = os.getenv("FEATBIT_SAMPLE_ENV_SECRET", "")
event_url = os.getenv("FEATBIT_SAMPLE_EVENT_URL", "http://localhost:5100")
streaming_url = os.getenv("FEATBIT_SAMPLE_STREAMING_URL", "ws://localhost:5100")
user_key = os.getenv("FEATBIT_SAMPLE_USER", "test-user-1")
user = {"key": user_key, "name": user_key}

set_config(Config(env_secret, event_url, streaming_url))

with get() as fb_client:
    if fb_client.is_enabled("runner-game", user):
        print(f"Dino Game is released to {user['name']}")
        flag_state = fb_client.variation_detail("difficulty-mode", user, "error")
        if flag_state.success:
            print(f"Dino Game is on {flag_state.data.variation} mode for {user['name']}")
    else:
        print(f"Dino Game not released to {user['name']}")
    # be sure to send events
    sleep(5)
