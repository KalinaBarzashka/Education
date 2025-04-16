import { createLazyFileRoute, Link } from '@tanstack/react-router' // use Link instead of <a> tag

export const Route = createLazyFileRoute('/')({
  component: RouteComponent,
})

function RouteComponent() {
  return (
    <div className="index">
      <div className="index-brand">
        <h1>Padre Gino's</h1>
        <p>Pizza & Art at a location near you</p>
      </div>
      <ul>
        <li>
          <Link to="/order">Order</Link>
        </li>
        <li>
          <Link to="/past">Past Orders</Link>
        </li>
      </ul>
    </div>
  );
}
