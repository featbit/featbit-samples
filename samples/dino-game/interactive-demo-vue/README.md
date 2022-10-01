## Install

```
npm install
```

## Run

```
npm run dev
```

## Store for VUE

This sample used Pinia as the Store for VUE. We wrapped FeatBit's feature flags in a pinia store named `useFeatBitStore` in file '/src/featbit.js'.  

This facilitates the usage of our feature flags. Every component can use feature flags through `useFeatBitStore`. This ensures that the feature flags can be used and updated in real-time, and all of this is managed in file '/src/featbit.js'

