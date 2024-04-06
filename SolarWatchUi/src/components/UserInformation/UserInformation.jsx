import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import "./UserInformation.css";

export const UserInformation = () => {
  const navigate = useNavigate();
  const [userInfo, setUserInfo] = useState({
    id: "",
    email: "",
    userName: "",
    phoneNumber: "",
    role: "",
  });

  const logedInUser = JSON.parse(localStorage.getItem("userInfo"));

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await fetch(
          `http://localhost:5071/Auth/userInformation/${logedInUser.email}`,
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${localStorage.getItem("token")}`,
            },
          }
        );

        if (response.status === 401) {
          toast.warn("Session expired.Please log in again.");
          navigate("/login");
        }

        const data = await response.json();

        setUserInfo({
          id: data.id,
          email: data.email,
          userName: data.username,
          phoneNumber: data.phoneNumber,
          role: data.role,
        });
      } catch (error) {
        console.error("Error:", error);
      }
    };

    fetchData();
  }, []);

  console.log(userInfo.role);
  return (
    <>
      {userInfo.role === "User" && (
        <div className="user-information-container">
          <h1>User Information</h1>
          <div className="col-lg-8">
            <div
              className="card mb-4"
              style={{
                background: "rgba(255, 255, 255, 0.1)",
                backdropFilter: "blur(10px)",
                color: "#FFFFF0",
              }}
            >
              <div className="card-body">
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">User name</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">{userInfo.userName}</p>
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">Email</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">{userInfo.email}</p>
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">Phone</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">{userInfo.phoneNumber}</p>
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">Role</p>
                  </div>
                  <div className="col-sm-9 ">
                    <p className="text-muted mb-0">{userInfo.role}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default UserInformation;
