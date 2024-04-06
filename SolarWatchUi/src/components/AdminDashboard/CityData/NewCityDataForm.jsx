import { toast } from "react-toastify";

const NewCityDataForm = ({
  newCityData,
  setNewCityData,
  cities,
  setCities,
  setIsAddingCity,
}) => {
  const handleAddCity = async (e) => {
    e.preventDefault();
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    try {
      const response = await fetch(
        "http://localhost:5071/CityData/AddCityData",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
          body: JSON.stringify(newCityData),
        }
      );
      if (response.ok) {
        const data = await response.json();
        setCities([...cities, data]);
        toast.success("City added successfully.");
        setIsAddingCity(false);
      } //response status check, role, expired token..
    } catch (error) {
      console.log("Error:", error);
      toast.error("An error occurred while adding the city.");
    }
  };

  return (
    <div className="add-data-form">
      <form>
        <h1 className="add-data-form-header">Add new city</h1>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="City name"
            name="cityName"
            id="cityName"
            autoComplete="on"
            className="form-control"
            value={newCityData.cityName}
            onChange={(e) =>
              setNewCityData({ ...newCityData, cityName: e.target.value })
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
            placeholder="Country"
            name="country"
            id="country"
            autoComplete="on"
            className="form-control"
            value={newCityData.country}
            onChange={(e) => {
              setNewCityData({ ...newCityData, country: e.target.value });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="State"
            name="state"
            id="state"
            className="form-control"
            value={newCityData.state}
            onChange={(e) => {
              setNewCityData({ ...newCityData, state: e.target.value });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="Latitude"
            name="latitude"
            id="latitude"
            className="form-control"
            value={newCityData.latitude}
            onChange={(e) => {
              setNewCityData({ ...newCityData, latitude: e.target.value });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <div className="input-group mb-3">
          <input
            type="text"
            placeholder="Longitude"
            name="longitude"
            id="longitude"
            className="form-control"
            value={newCityData.longitude}
            onChange={(e) => {
              setNewCityData({ ...newCityData, longitude: e.target.value });
            }}
            style={{
              backgroundColor: "rgba(255, 255, 255, 0.1)",
              borderColor: "rgba(255, 255, 255, 0.1)",
            }}
          />
        </div>

        <button
          type="submit"
          onClick={(e) => handleAddCity(e)}
          style={{
            marginBottom: "1rem",
          }}
        >
          Save
        </button>
        <button onClick={() => setIsAddingCity(false)}>Cancle</button>
      </form>
    </div>
  );
};

export default NewCityDataForm;
