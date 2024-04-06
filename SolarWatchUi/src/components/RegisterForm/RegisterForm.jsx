import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./RegisterForm.css";
import { toast } from "react-toastify";

const RegisterForm = () => {
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");

  const navigate = useNavigate();

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleUsernameChange = (e) => {
    setUsername(e.target.value);
  };

  const handlePasswordChange = (e) => {
    setPassword(e.target.value);
  };

  const handleRepeatPasswordChange = (e) => {
    setRepeatPassword(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (password !== repeatPassword) {
      toast.warn("Passwords does not match.")
      return;
    }
    try {
      const response = await fetch("http://localhost:5071/Auth/Register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email,
          username,
          password,
        }),
      });
      if (response.ok) {
        toast.success("Successfully registered!");
        navigate("/login");
      } else {
        
        throw new Error("Error: " + response.status);
      }
      
    } catch (error) {
      toast.error("Something went wrong.");
      console.error("Error:", error);
    }
  };

  return (
    <div className="register-form">
      <form onSubmit={handleSubmit}>
        <h1 className="register-form-header">Register</h1>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="Email"
            name="email"
            id="email"
            autoComplete="on"
            className="form-control"
            value={email}
            onChange={handleEmailChange}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="Username"
            name="username"
            id="username"
            autoComplete="on"
            className="form-control"
            value={username}
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

        <div className="input-group mb-3">
          <input
            type="password"
            placeholder="Repeat Password"
            name="repeatPassword"
            id="repeatPassword"
            className="form-control"
            value={repeatPassword}
            onChange={handleRepeatPasswordChange}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <button type="submit" className="register-form-button">
          Register
        </button>
      </form>
    </div>
  );
};

export default RegisterForm;
