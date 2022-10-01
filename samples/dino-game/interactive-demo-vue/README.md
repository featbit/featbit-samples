
## Install

```
npm install
```

## Run

```
npm run dev
```

## Main dependencies and versions of project

We created the sample with `npm create vite@latest interactive-demo-vue -- --template vue`.

We used `antdv` for UI components. 

We used `Pinia` for Store of VUE.

<img width="500" alt="2022-04-09_122409" src="https://user-images.githubusercontent.com/68597908/193397108-2d63d927-fe38-4e28-9e09-448d669b1dfc.png">


## Store for VUE

This sample used Pinia as the Store for VUE. We wrapped FeatBit's feature flags in a pinia store named `useFeatBitStore` in file '/src/featbit.js'.  

This facilitates the usage of our feature flags. Every component can use feature flags through `useFeatBitStore`. This ensures that the feature flags can be used and updated in real-time, and all of this is managed in file '/src/featbit.js'

