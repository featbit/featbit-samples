package co.featbit.demo;

import co.featbit.commons.model.FBUser;
import co.featbit.server.exterior.FBClient;
import lombok.RequiredArgsConstructor;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/demo/api/java")
@RequiredArgsConstructor
public class GameDemoController {

    private final FBClient client;

    @GetMapping("/{user}")
    public String isReleasedGame(@PathVariable String user) {
        var u = new FBUser.Builder(user).userName(user).build();
        return client.isEnabled("runner-game", u) ? String.format("Dino Game is released to %s", user) : String.format("Dino Game not released to %s", user);
    }

    @GetMapping("/{user}/difficulty")
    public String getDifficultyMode(@PathVariable String user) {
        var u = new FBUser.Builder(user).userName(user).build();
        if (!client.isEnabled("runner-game", u)) return String.format("Dino Game not released to %s", user);
        return switch (client.variation("difficulty-mode", u, "normal")) {
            case "hard" -> String.format("Dino Game is on hard mode for %s", user);
            case "easy" -> String.format("Dino Game is on easy mode for %s", user);
            default -> String.format("Dino Game is on normal mode for %s", user);
        };
    }

}
