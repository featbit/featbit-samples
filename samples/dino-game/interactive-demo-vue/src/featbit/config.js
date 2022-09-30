export const flagsDefaultValues = [
    // Set default values for all the flags which would be used
    // Even thought it should never happen, this makes sure that the app can still be running even if the network connection was lost between the client and the ffc server
    // Be aware, this is not necessary, you can leave the object empty.
    {
        id: "difficulty-mode",
        variation: "easy",
        variationType: "string"
    },
    {
        id: "game-runner",
        variation: "false",
        variationType: "boolean"
    }
]

export const option = {
    secret: "ZTczLTFiMTctNCUyMDIyMDkyOTA1MDUwOV9fMTU5X18yMzVfXzQ1MV9fZGVmYXVsdF9lY2RjMA==", // replace with your won secret
    anonymous: false,
    user: { // you can keep this
        id: 'my-user',
        userName: 'my user',
        email: '',
        customizedProperties: [
            {
                "name": "sex",
                "value": "male"
            }]
    },
    devModePassword: 'thisisademo'
}




