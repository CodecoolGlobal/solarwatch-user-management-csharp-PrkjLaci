import { BrowserRouter, Route, Routes, Link } from "react-router-dom";
import { useState } from "react";

import "./App.css";
import Home from "./components/Home/Home";
import LoginFrom from "./components/LoginForm/LoginForm";
import RegisterForm from "./components/RegisterForm/RegisterForm";
import NavBar from "./components/NavBar/NavBar";
import SolarWatch from "./components/SolarWatch/SolarWatch";
import CityData from "./components/AdminDashboard/CityData/CityData";
import UserInformation from "./components/UserInformation/UserInformation";
import SunsetSunriseData from "./components/AdminDashboard/SunsetSunriseData/SunsetSunriseData";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isAdminLoggedIn, setAdminLoggedIn] = useState(false);

  return (
    <BrowserRouter>
      <header>
        <NavBar isLoggedIn={isLoggedIn} isAdminLoggedIn={isAdminLoggedIn} />
      </header>
      <main className="app">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route
            path="/login"
            element={
              <LoginFrom
                setIsLoggedIn={setIsLoggedIn}
                setAdminLoggedIn={setAdminLoggedIn}
              />
            }
          />
          <Route path="/signup" element={<RegisterForm />} />
          <Route path="/user-information" element={<UserInformation />} />
          <Route
            path="/solar-watch"
            element={<SolarWatch setIsLoggedIn={setIsLoggedIn} />}
          />
          <Route path="/admin-dashboard-city-data" element={<CityData />} />

          <Route
            path="/admin-dashboard-sunset-sunrise-data"
            element={<SunsetSunriseData />}
          />
        </Routes>
      </main>
      <footer className="fixed-bottom">
        <div className="text-center p-3">© 2024 solarwatch.com</div>
      </footer>
    </BrowserRouter>
  );
}

export default App;
