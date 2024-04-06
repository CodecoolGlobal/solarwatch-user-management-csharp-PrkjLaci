import { MdDeleteForever } from "react-icons/md";
import { PiGearBold } from "react-icons/pi";
import { FaRegSave } from "react-icons/fa";
import { RiArrowGoBackLine } from "react-icons/ri";
import { useState } from "react";
import { toast } from "react-toastify";

const CityDataTable = ({ cities, setCities }) => {
  const [addingRowId, setAddingRowId] = useState(null);
  const [cityData, setCityData] = useState({
    cityName: "",
    latitude: "",
    longitude: "",
    country: "",
    state: "",
  });

  const handleDeleteCity = async (cityId) => {
    try {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      const response = await fetch(
        `http://localhost:5071/CityData/DeleteCityData/${cityId}`,
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
        }
      );
      if (response.ok) {
        const updatedCitiesResponse = await fetch(
          "http://localhost:5071/CityData/GetAllCityData",
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${userInfo.token}`,
            },
          }
        );
        const updatedCitiesData = await updatedCitiesResponse.json();
        setCities(updatedCitiesData.$values);
        toast.success("Deleted city successfully.");
        setAddingRowId(null);
      }
    } catch (error) {
      console.error("Error:", error);
      toast.error("An error occurred while deleting the city.");
    }
  };

  const handleUpdateCityData = async () => {
    try {
      const userInfo = JSON.parse(localStorage.getItem("userInfo"));
      const response = await fetch(
        "http://localhost:5071/CityData/UpdateCityData",
        {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
          body: JSON.stringify({
            id: cityData.id,
            cityName: cityData.cityName,
            country: cityData.country,
            state: cityData.state,
            latitude: cityData.latitude,
            longitude: cityData.longitude,
          }),
        }
      );

      if (response.ok) {
        const updatedCitiesResponse = await fetch(
          "http://localhost:5071/CityData/GetAllCityData",
          {
            method: "GET",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${userInfo.token}`,
            },
          }
        );
        const updatedCitiesData = await updatedCitiesResponse.json();
        setCities(updatedCitiesData.$values);
        toast.success("City data updated successfully.");
        setAddingRowId(null);
      }

      if (response.status === 401) {
        toast.warn("Session expired.Please log in again.");
        nav("/login");
        throw new Error("Network response was not ok");
      }
    } catch (error) {
      console.error("Error:", error);
    }
  };

  const handleModifyCity = async (cityId) => {
    setAddingRowId(cityId);
    const selectedCity = cities.find((city) => city.id === cityId);
    setCityData(selectedCity);
  };

  const handleCloseCityDataForm = () => {
    setAddingRowId(null);
  };

  return (
    <table className="table table-striped">
      <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">City name</th>
          <th scope="col">Country</th>
          <th scope="col">State</th>
          <th scope="col">Latitude</th>
          <th scope="col">Longitude</th>
          <th scope="col">Actions</th>
        </tr>
      </thead>
      <tbody>
        {cities.map((city, index) => (
          <>
            <tr
              key={city.id}
              id="new-city-row,"
              style={{
                backgroundColor: `rgba(255, 255, 255, ${
                  index % 2 === 0 ? 0.1 : 0.3
                })`,
              }}
            >
              <th scope="row">{index + 1}</th>
              <td>{city.cityName}</td>
              <td>{city.country}</td>
              <td>{city.state}</td>
              <td>{city.latitude}</td>
              <td>{city.longitude}</td>
              <td>
                <PiGearBold
                  className="modify-icon"
                  onClick={() => handleModifyCity(city.id)}
                />
                <MdDeleteForever
                  className="delete-icon"
                  onClick={() => handleDeleteCity(city.id)}
                />
              </td>
            </tr>
            {addingRowId === city.id && (
              <tr className="data-to-modify">
                <th scope="row">{index + 1}</th>
                <td>
                  <input
                    type="text"
                    value={cityData.cityName}
                    placeholder={city.cityName}
                    onChange={(e) =>
                      setCityData({
                        ...cityData,
                        cityName: e.target.value,
                      })
                    }
                    style={{
                      width: "60%",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={cityData.country}
                    placeholder={city.country}
                    onChange={(e) =>
                      setCityData({
                        ...cityData,
                        country: e.target.value,
                      })
                    }
                    style={{
                      width: "30%",
                      backgroundColor: "rgba(255, 255, 255, 0.1)",
                      borderColor: "#FFFFF0",
                      color: "#FFFFF0",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={cityData.state}
                    placeholder={city.state}
                    onChange={(e) =>
                      setCityData({
                        ...cityData,
                        state: e.target.value,
                      })
                    }
                    style={{
                      width: "60%",
                      backgroundColor: "rgba(255, 255, 255, 0.1)",
                      borderColor: "#FFFFF0",
                      color: "#FFFFF0",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={cityData.latitude}
                    placeholder={city.latitude}
                    onChange={(e) =>
                      setCityData({
                        ...cityData,
                        latitude: e.target.value,
                      })
                    }
                    style={{
                      width: "70%",
                      backgroundColor: "rgba(255, 255, 255, 0.1)",
                      borderColor: "#FFFFF0",
                      color: "#FFFFF0",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={cityData.longitude}
                    placeholder={city.longitude}
                    onChange={(e) =>
                      setCityData({
                        ...cityData,
                        longitude: e.target.value,
                      })
                    }
                    style={{
                      width: "70%",
                      backgroundColor: "rgba(255, 255, 255, 0.1)",
                      borderColor: "#FFFFF0",
                      color: "#FFFFF0",
                    }}
                  />
                </td>
                <td>
                  <FaRegSave
                    className="save-icon"
                    onClick={() => handleUpdateCityData()}
                  />
                  <RiArrowGoBackLine
                    className="close-icon"
                    onClick={() => handleCloseCityDataForm()}
                  />
                </td>
              </tr>
            )}
          </>
        ))}
      </tbody>
    </table>
  );
};

export default CityDataTable;
