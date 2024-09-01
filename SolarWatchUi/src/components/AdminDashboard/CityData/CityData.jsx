import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import "../../../index.css";

import NewCityDataForm from "./NewCityDataForm";
import CityDataTable from "./CityDataTable";

const CityData = () => {
  const nav = useNavigate();
  const [cities, setCities] = useState([]);
  const [isAddingCity, setIsAddingCity] = useState(false);
  const userInfo = JSON.parse(localStorage.getItem("userInfo"));

  const fetchCityData = async () => {
    try {
      const response = await fetch(
        "http://localhost:5071/CityData/GetAllCityData",
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
        }
      );
      if (response.status === 401) {
        toast.warn("Session expired.Please log in again.");
        nav("/login");
        throw new Error("Network response was not ok");
      }

      const data = await response.json();
      if (JSON.stringify(data.$values) !== JSON.stringify(cities)) {
        setCities(data.data.$values);
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  useEffect(() => {
    fetchCityData();
  }, []);
  return (
    <div className="data-container">
      <h1>City Data</h1>
      <CityDataTable cities={cities} setCities={setCities} />
      {isAddingCity ? (
        <NewCityDataForm
          setIsAddingCity={setIsAddingCity}
          setCities={setCities}
          cities={cities}
        />
      ) : (
        <>
          <button onClick={() => setIsAddingCity(true)}>
            Add new city data
          </button>
        </>
      )}
    </div>
  );
};

export default CityData;
