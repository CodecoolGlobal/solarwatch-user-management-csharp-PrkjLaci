import { Link } from "react-router-dom";
import ToastMessage from "../Toast/ToastMessage";
import "./NavBar.css";

const NavBar = ({ isLoggedIn, isAdminLoggedIn }) => {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div
        className="collapse navbar-collapse container-fluid justify-content-center"
        id="navbarNav"
      >
        <ul className="navbar-nav">
          <li className="nav-item active">
            <Link to="/" className="nav-link">
              Home <span className="sr-only">(current)</span>
            </Link>
          </li>

          <li className="nav-item">
            {isLoggedIn ? (
              <Link to="/solar-watch" className="nav-link">
                Solar Watch <span className="sr-only">(current)</span>
              </Link>
            ) : (
              <Link className="nav-link disabled">SolarWatch</Link>
            )}
          </li>

          {isLoggedIn ? (
            isAdminLoggedIn ? (
              <li className="nav-item dropdown">
                <Link
                  className="nav-link dropdown-toggle"
                  data-bs-toggle="dropdown"
                  href="#"
                  role="button"
                  aria-expanded="false"
                >
                  Admin dashboard
                </Link>
                <ul className="dropdown-menu">
                  <Link className="dropdown-item" to={"/admin-dashboard-user"}>
                    User dashboard
                  </Link>
                  <Link
                    className="dropdown-item"
                    to={"/admin-dashboard-city-data"}
                  >
                    City dashboard
                  </Link>
                  <Link
                    className="dropdown-item"
                    to={"/admin-dashboard-sunset-sunrise-data"}
                  >
                    Sunset sunrise dashboard
                  </Link>
                </ul>
              </li>
            ) : (
              <li className="nav-item">
                <Link to="/user-information" className="nav-link">
                  User Information <span className="sr-only"></span>
                </Link>
              </li>
            )
          ) : (
            <li className="nav-item">
              <Link to="/login" className="nav-link">
                Login/Sign up <span className="sr-only">(current)</span>
              </Link>
            </li>
          )}

          <li className="nav-item">
            <Link to="/about" className="nav-link">
              About <span className="sr-only">(current)</span>
            </Link>
          </li>
        </ul>
        <ToastMessage />
      </div>
    </nav>
  );
};

export default NavBar;
