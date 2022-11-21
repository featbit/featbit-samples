package co.featbit.demo;

import co.featbit.commons.model.FBUser;
import co.featbit.server.FBClientImp;
import co.featbit.server.FBConfig;
import co.featbit.server.exterior.FBClient;

import java.io.IOException;

public class JavaDemo {
    public static void main(String[] args) throws IOException {
        var envSecret = System.getProperty("fb-env-secret");
        var streamUrl = System.getProperty("fb-streaming-url");
        var eventUrl = System.getProperty("fb-event-url");
        var user = System.getProperty("fb-user");

        FBConfig config = new FBConfig.Builder()
                .eventURL(eventUrl)
                .streamingURL(streamUrl)
                .build();
        var u = new FBUser.Builder(user)
                .userName(user)
                .build();
        FBClient client = new FBClientImp(envSecret, config);
        if (client.isInitialized()) {
            if (client.isEnabled("runner-game", u)) {
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
        client.close();
        System.out.println("APP FINISHED");
    }
}
