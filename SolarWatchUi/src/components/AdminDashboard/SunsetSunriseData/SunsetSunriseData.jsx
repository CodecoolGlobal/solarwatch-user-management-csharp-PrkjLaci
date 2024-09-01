import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import SunsetSunriseDataTable from "./SunsetSunriseDataTable";
import NewSunsetSunriseForm from "./NewSunsetSunriseForm";
import "../../../index.css";

const SunsetSunriseData = () => {
  const nav = useNavigate();
  const userInfo = JSON.parse(localStorage.getItem("userInfo"));
  const [sunsetSunrises, setSunsetSunrises] = useState([]);
  const [isAddingData, setIsAddingData] = useState(false);

  const fetchSunsetSunriseData = async () => {
    try {
      const response = await fetch(
        "http://localhost:5071/SunsetSunrise/GetAllSunsetSunrise",
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
      }

      if (response.ok) {
        const data = await response.json();
        setSunsetSunrises(data.data.$values);
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  useEffect(() => {
    fetchSunsetSunriseData();
  }, []);

  return (
    <div className="data-container">
      <h1>Sunset Sunrise data</h1>
      <SunsetSunriseDataTable
        sunsetSunrises={sunsetSunrises}
        setSunsetSunrises={setSunsetSunrises}
      />
      {isAddingData ? (
        <NewSunsetSunriseForm
          setIsAddingData={setIsAddingData}
          setSunsetSunrises={setSunsetSunrises}
          sunsetSunrises={sunsetSunrises}
        />
      ) : (
        <>
          <button onClick={() => setIsAddingData(true)}>
            Add new sunset sunrise data
          </button>
        </>
      )}
    </div>
  );
};
export default SunsetSunriseData;
