import { useState, useEffect } from "react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import "./SolarWatch.css";

const SolarWatch = ({ setIsLoggedIn }) => {
  const navigate = useNavigate();
  const [cityName, setCityName] = useState("");
  const [date, setDate] = useState("");
  const [cityData, setCityData] = useState({
    cityName: "",
    country: "",
    state: "",
    latitude: "",
    longitude: "",
  });
  const [sunsetSunrise, setsunsetSunrise] = useState({
    date: "",
    sunrise: "",
    sunset: "",
  });
  const [isSubmited, setIsSubmited] = useState(false);

  const handleCityChange = (e) => {
    setCityName(e.target.value);
  };

  const handleDateChange = (e) => {
    setDate(e.target.value);
  };

  console.log(cityName, date);

  const handleSubmit = async (e) => {
    try {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      e.preventDefault();
      const response = await fetch(
        `http://localhost:5071/SunsetSunrise/GetSunsetSunrise?city=${cityName}&date=${date}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
        }
      );
      if (response.ok) {
        toast.success("Data retrieved.");
      }

      if (!response.ok) {
        toast.error("Something went wrong!\nCheck the entered data!");
      }

      if (!response.ok && response.status === 401) {
        setIsLoggedIn(false);
        toast.warn("Session expired.Please log in again.");
        navigate("/login");
        throw new Error("Network response was not ok");
      }

      const data = await response.json();
      setCityData({
        cityName: data.data.city.cityName,
        country: data.data.city.country,
        state: data.data.city.state,
        latitude: data.data.city.latitude,
        longitude: data.data.city.longitude,
      });
      setsunsetSunrise({
        date: data.data.date,
        sunrise: data.data.sunrise,
        sunset: data.data.sunset,
      });
      setIsSubmited(true);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  console.log(cityData, sunsetSunrise, isSubmited);

  return (
    <div className="solar-watch-container">
      <div className="solar-watch-form">
        <form onSubmit={handleSubmit}>
          <label htmlFor="cityName">City name</label>
          <div className="input-group mb-3">
            <input
              type="string"
              className="form-control"
              id="cityName"
              aria-describedby="emailHelp"
              placeholder="Enter city name"
              required
              onChange={(e) => handleCityChange(e)}
              style={{
                backgroundColor: "rgba(255, 255, 255, 0.1)",
                borderColor: "rgba(255, 255, 255, 0.1)",
              }}
            />
          </div>

          <label htmlFor="date">Date</label>
          <div className="input-group mb-3">
            <input
              type="date"
              className="form-control datepicker"
              id="date"
              required
              onChange={(e) => handleDateChange(e)}
              style={{
                backgroundColor: "rgba(255, 255, 255, 0.1)",
                borderColor: "rgba(255, 255, 255, 0.1)",
                color: "black",
              }}
            />
          </div>
          <button type="submit">
            Submit
          </button>
        </form>
      </div>
      {isSubmited && (
        <div className="sunset-sunrise-information-container">
          <h1>Sunset and Sunrise:</h1>
          <div className="col-lg-12">
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
                    <p className="mb-0">City</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">
                      {cityData.cityName ? cityData.cityName : cityName}
                    </p>
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">Date</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">{sunsetSunrise.date}</p>
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">Sunrise</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">{sunsetSunrise.sunrise}</p>
                  </div>
                </div>
                <hr />
                <div className="row">
                  <div className="col-sm-3">
                    <p className="mb-0">Sunset</p>
                  </div>
                  <div className="col-sm-9">
                    <p className="text-muted mb-0">{sunsetSunrise.sunset}</p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default SolarWatch;
