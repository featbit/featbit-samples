package main

import (
	"fmt"
	"github.com/featbit/featbit-go-sdk"
	"github.com/featbit/featbit-go-sdk/interfaces"
	"os"
)

func main() {
	envSecret := os.Getenv("FEATBIT_SAMPLE_ENV_SECRET")
	if envSecret == "" {
		return
	}
	streamingUrl := os.Getenv("FEATBIT_SAMPLE_STREAMING_URL")
	if streamingUrl == "" {
		streamingUrl = "ws://localhost:5100"
	}
	eventUrl := os.Getenv("FEATBIT_SAMPLE_STREAMING_URL")
	if eventUrl == "" {
		eventUrl = "http://localhost:5100"
	}

	userKey := os.Getenv("FEATBIT_SAMPLE_USER")
	if userKey == "" {
		userKey = "test-user-1"
	}

	client, err := featbit.NewFBClient(envSecret, streamingUrl, eventUrl)
	if err != nil {
		return
	}
	if client.IsInitialized() {
		user, _ := interfaces.NewUserBuilder(userKey).Build()
		if res, _, _ := client.BoolVariation("runner-game", user, false); res {
			fmt.Printf("Dino Game is released to %s \n", userKey)
			mode, _, _ := client.Variation("difficulty-mode", user, "normal")
			switch mode {
			case "hard":
				fmt.Printf("Dino Game is on hard mode for %s \n", userKey)
			case "easy":
				fmt.Printf("Dino Game is on easy mode for %s \n", userKey)
			default:
				fmt.Printf("Dino Game is on normal mode for %s \n", userKey)
			}
		} else {
			fmt.Printf("Dino Game is not released to %s \n", userKey)
		}

	}
}
