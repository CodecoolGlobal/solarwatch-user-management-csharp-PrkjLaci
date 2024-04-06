import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

import "../../../index.css";

const newSunsetSunriseFrom = ({
  newSunsetSunriseData,
  setNewSunsetSunriseData,
  setIsAddingData,
  setSunsetSunrises,
  sunsetSunrises,
}) => {
    const nav = useNavigate();

  const handleAddSunsetSunrise = async (e) => {
    e.preventDefault();
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    try {
      const response = await fetch(
        "http://localhost:5071/SunsetSunrise/AddSunsetSunrise",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
          body: JSON.stringify(newSunsetSunriseData),
        }
    );
    if(response.ok) {
        const data = response.json();
        setSunsetSunrises([... sunsetSunrises, data]);
        toast.success("Sunset sunrise data added succesffuly!");
        setIsAddingData(false);
    } else if(response.status === 400) {
        toast.error("Sunset sunrise data already added!")
    } else if(response.status === 401) {
        toast.warn("Your session has expired. Please log in again.")
        nav("/login");
    }
    } catch (error) {
        toast.warn(error)
    }
  };

  return (
    <div className="add-data-form">
      <form>
        <h1 className="add-data-form-header">Add new sunset sunrise</h1>

        <div className="input-group mb-3">
          <input
            type="date"
            placeholder="Date"
            name="date"
            id="date"
            autoComplete="on"
            className="form-control"
            value={newSunsetSunriseData.date}
            onChange={(e) =>
              setNewSunsetSunriseData({
                ...newSunsetSunriseData,
                date: e.target.value,
              })
            }
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="Sunrise"
            name="sunrise"
            id="sunrise"
            autoComplete="on"
            className="form-control"
            value={newSunsetSunriseData.sunrise}
            onChange={(e) => {
              setNewSunsetSunriseData({
                ...newSunsetSunriseData,
                sunrise: e.target.value,
              });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="string"
            placeholder="Sunset"
            name="sunset"
            id="sunset"
            className="form-control"
            value={newSunsetSunriseData.sunset}
            onChange={(e) => {
              setNewSunsetSunriseData({
                ...newSunsetSunriseData,
                sunset: e.target.value,
              });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="number"
            placeholder="City ID"
            name="cityId"
            id="cityId"
            className="form-control"
            value={newSunsetSunriseData.cityId}
            onChange={(e) => {
              setNewSunsetSunriseData({
                ...newSunsetSunriseData,
                cityId: e.target.value,
              });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <button
          type="submit"
          onClick={(e) => handleAddSunsetSunrise(e)}
          style={{
            marginBottom: "1rem",
          }}
        >
          Save
        </button>
        <button onClick={() => setIsAddingData(false)}>Cancle</button>
      </form>
    </div>
  );
};

export default newSunsetSunriseFrom;
