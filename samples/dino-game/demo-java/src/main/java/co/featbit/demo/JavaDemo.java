package co.featbit.demo;

import co.featbit.commons.model.FBUser;
import co.featbit.server.FBClientImp;
import co.featbit.server.FBConfig;
import co.featbit.server.exterior.FBClient;

import java.io.IOException;

import static java.lang.Thread.sleep;

public class JavaDemo {
    public static void main(String[] args) throws IOException, InterruptedException {
        var envVars = System.getenv();
        var envSecret = envVars.get("FEATBIT_SAMPLE_ENV_SECRET");
        var streamUrl = envVars.getOrDefault("FEATBIT_SAMPLE_EVENT_URL", "ws://localhost:5100");
        var eventUrl = envVars.getOrDefault("FEATBIT_SAMPLE_STREAMING_URL", "http://localhost:5100");
        var user = envVars.getOrDefault("FEATBIT_SAMPLE_USER", "test-user-1");

        FBConfig config = new FBConfig.Builder()
                .eventURL(eventUrl)
                .streamingURL(streamUrl)
                .build();
        FBUser u = new FBUser.Builder(user)
                .userName(user)
                .build();
        FBClient client = new FBClientImp(envSecret, config);
        if (client.isInitialized()) {
            if (client.boolVariation("runner-game", u, false)) {
                System.out.println(String.format("Dino Game is released to %s", u.getUserName()));
                var difficulty = switch (client.variation("difficulty-mode", u, "normal")) {
                    case "hard" -> String.format("Dino Game is on hard mode for %s", u.getUserName());
                    case "easy" -> String.format("Dino Game is on easy mode for %s", u.getUserName());
                    default -> String.format("Dino Game is on normal mode for %s", u.getUserName());
                };
                System.out.println(difficulty);
            } else {
                System.out.println(String.format("Dino Game not released to %s", u.getUserName()));
            }
        }
        // be sure to send events
        sleep(5000);
        client.close();
        System.out.println("APP FINISHED");
    }
}
