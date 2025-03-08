import React from "react";
import { createRoot } from "react-dom/client";
// create out first app component here:
const Pizza = (props) => {
  return React.createElement("div", {}, [
    // return multiple top level React components
    React.createElement("h1", {}, props.name),
    React.createElement("p", {}, props.description),
  ]);
};

const App = () => {
  return React.createElement("div", {}, [
    React.createElement("h1", {}, "Padre Gino's"),
    React.createElement(Pizza, {
      name: "The Peperronni pizza 1",
      description: "Mushrooms with Pineapple abd Pepperoni 1",
    }),
    React.createElement(Pizza, {
      name: "The Peperronni pizza 2",
      description: "Mushrooms with Pineapple abd Pepperoni 2",
    }),
    React.createElement(Pizza, {
      name: "The Peperronni pizza 3",
      description: "Mushrooms with Pineapple abd Pepperoni 3",
    }),
    React.createElement(Pizza, {
      name: "The Peperronni pizza 4",
      description: "Mushrooms with Pineapple abd Pepperoni 4",
    }),
    React.createElement(Pizza, {
      name: "The Peperronni pizza 5",
      description: "Mushrooms with Pineapple abd Pepperoni 5",
    }),
  ]);
};

const container = document.getElementById("root");
const root = createRoot(container);
root.render(React.createElement(App));
