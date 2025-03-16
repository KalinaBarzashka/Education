import { createRoot } from "react-dom/client";
import Pizza from "./Pizza";

const App = () => {
  return(
    <div>
      <h1>Padre Gino's - Order Now</h1>
      <Pizza name="The Pepperoni Pizza"
        description="Mozzarella Cheese, Pepperoni"
        image={"/public/pizzas/pepperoni.webp"} />
      <Pizza name="The Barbecue Chicken Pizza"
        description="Barbecued Chicken, Red Peppers, Green Peppers, Tomatoes, Red Onions, Barbecue Sauce"
        image={"/public/pizzas/bbq_ckn.webp"} />
      <Pizza name="The California Chicken Pizza"
        description="Chicken, Artichoke, Spinach, Garlic, Jalapeno Peppers, Fontina Cheese, Gouda Cheese"
        image={"/public/pizzas/cali_ckn.webp"} />
    </div>
  )
};

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);
