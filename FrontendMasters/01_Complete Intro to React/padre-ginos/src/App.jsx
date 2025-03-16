import { createRoot } from "react-dom/client";
import Pizza from "./Pizza";

const App = () => {
  return(
    <div>
      <h1>Padre Gino's - Order Now</h1>
      <Pizza name="Pepperoni" description="Peperoni, cheese, etc..." />
      <Pizza name="Hawaiian" description="Ham, pineapple, etc..." />
      <Pizza name="Americano" description="French fries, hot dog, etc..." />
    </div>
  )
};

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);
