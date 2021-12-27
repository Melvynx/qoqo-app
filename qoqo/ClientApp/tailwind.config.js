/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-var-requires */
const { guessProductionMode } = require("@ngneat/tailwind");

module.exports = {
  // Material-angular is so bad, so we have to override rules
  important: true,
  prefix: "",
  purge: {
    enabled: guessProductionMode(),
    content: ["./src/**/*.{html,ts}"],
  },
  darkMode: "class", // or 'media' or 'class'
  theme: {
    extend: {},
  },
  variants: {
    extend: {},
  },
  plugins: [],
};
