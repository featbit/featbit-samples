import os
from fbclient import set_config, get
from fbclient.config import Config

env_secret = os.getenv("FEATBIT_SAMPLE_ENV_SECRET", "")
event_url = os.getenv("FEATBIT_SAMPLE_EVENT_URL", "http://localhost:5100")
streaming_url = os.getenv("FEATBIT_SAMPLE_STREAMING_URL", "ws://localhost:5100")
user_key = os.getenv("FEATBIT_SAMPLE_USER", "test-user-1")
user = {"key": user_key, "name": user_key}

set_config(Config(env_secret, event_url, streaming_url))

with get() as fb_client:
    if fb_client.variation("runner-game", user, False) is True:
        print(f"Dino Game is released to {user['name']}")
        detail = fb_client.variation_detail("difficulty-mode", user, "error")
        if detail.variation_id is not None:
            print(f"Dino Game is on {detail.variation} mode for {user['name']}")
        else:
            print(f"error: {detail.reason}")
    else:
        print(f"Dino Game not released to {user['name']}")
    # Exiting the context stops the SDK and attempts to flush pending events.
