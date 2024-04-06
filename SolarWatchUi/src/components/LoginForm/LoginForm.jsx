import React, { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { Link, useNavigate } from "react-router-dom";
import "./LoginForm.css";

const LoginForm = ({ setIsLoggedIn, setAdminLoggedIn }) => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleUsernameChange = (e) => {
    setEmail(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPassword(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const handleNavigate = (route) => {
      navigate(route);
    };

    try {
      const response = await fetch("http://localhost:5071/Auth/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email,
          password,
        }),
      });

      if (response.ok) {
        const data = await response.json();
        setIsLoggedIn(true);
        handleNavigate("/");
        localStorage.setItem("userInfo", JSON.stringify(data));
        data.role === "Admin" ? setAdminLoggedIn(true) : setAdminLoggedIn(false);
        toast.success("Login Successful!");

      } else {
        toast.error("Failed to login.");
      }
    } catch (error) {
      console.error("Error:", error);
      toast.error("An error occurred while logging in.");
    }
  };


  return (
    <div className="login-container">
      <div className="login-form">
        <form onSubmit={handleSubmit}>
          <h1 className="login-form-header">Login</h1>

          <div className="input-group mb-3">
            <input
              type="text"
              placeholder="Email"
              name="email"
              id="email"
              autoComplete="on"
              className="form-control"
              value={email}
              onChange={handleUsernameChange}
              style={{
                backgroundColor: "rgba(255, 255, 255, 0.1)",
                borderColor: "rgba(255, 255, 255, 0.1)",
              }}
            />
          </div>

          <div className="input-group mb-3">
            <input
              type="password"
              placeholder="Password"
              name="password"
              id="password"
              className="form-control"
              value={password}
              onChange={handlePasswordChange}
              style={{
                backgroundColor: "rgba(255, 255, 255, 0.1)",
                borderColor: "rgba(255, 255, 255, 0.1)",
              }}
            />
          </div>

          <button type="submit">
            Login
          </button>
          <div className="register">
            <p>
              Don't have an account? &nbsp;
              <Link
                to="/signup"
                className="register-link"
                style={{ textDecoration: "none" }}
              >
                Register here
              </Link>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default LoginForm;
