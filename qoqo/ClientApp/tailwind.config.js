/* eslint-disable no-undef */
/* eslint-disable @typescript-eslint/no-var-requires */
const { guessProductionMode } = require("@ngneat/tailwind");

module.exports = {
  prefix: "",
  purge: {
    enabled: guessProductionMode(),
    content: ["./src/**/*.{html,ts}"],
  },
  darkMode: "class", // or 'media' or 'class'
  theme: {
    extend: {
      animation: {
        "fade-in": "fadeIn 2000ms cubic-bezier(0.2, 0.2, 0.2, 0.9) backwards",
      },
      keyframes: {
        fadeIn: {
          "0%": { opacity: "0" },
          "100%": { opacity: "1" },
        },
      },
    },
  },
  variants: {
    extend: {},
  },
  plugins: [],
};
