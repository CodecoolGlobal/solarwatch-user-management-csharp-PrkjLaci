import { MdDeleteForever } from "react-icons/md";
import { PiGearBold } from "react-icons/pi";
import { FaRegSave } from "react-icons/fa";
import { RiArrowGoBackLine } from "react-icons/ri";
import { useState } from "react";
import { toast } from "react-toastify";

const SunsetSunriseDataTable = ({ sunsetSunrises, setSunsetSunrises }) => {
  const [addingRowId, setAddingRowId] = useState(null);
  const [sunsetSunriseData, setSunsetSunriseData] = useState({
    id: "",
    date: "",
    sunrise: "",
    sunset: "",
    cityId: "",
  });
  const handleDeleteSunsetSunrise = async (id) => {
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    const response = await fetch(
      `http://localhost:5071/SunsetSunrise/DeleteSunsetSunrise/${id}`,
      {
        method: "DELETE",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${userInfo.token}`,
        },
      }
    );
    if (response.ok) {
      const updatedSunsetSunrisesResponse = await fetch(
        "http://localhost:5071/SunsetSunrise/GetAllSunsetSunrise",
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
        }
      );
      const updatedSunsetSunrisesData =
        await updatedSunsetSunrisesResponse.json();
        console.log(updatedSunsetSunrisesData);
      setSunsetSunrises(updatedSunsetSunrisesData.data.$values);
      toast.success("Deleted sunset sunrise successfully.");
      setAddingRowId(null);
    }
  };

  console.log(sunsetSunriseData);

  const handleUpdateSunsetSunrise = async () => {
    const userInfo = JSON.parse(localStorage.getItem("userInfo"));
    const response = await fetch(
      `http://localhost:5071/SunsetSunrise/UpdateSunsetSunrise`,
      {
        method: "PATCH",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${userInfo.token}`,
        },
        body: JSON.stringify({
          id: sunsetSunriseData.id,
          date: sunsetSunriseData.date,
          sunrise: sunsetSunriseData.sunrise,
          sunset: sunsetSunriseData.sunset,
          cityId: sunsetSunriseData.cityId,
        }),
      }
    );

    if (response.ok) {
      const updatedSunsetSunrisesResponse = await fetch(
        "http://localhost:5071/SunsetSunrise/GetAllSunsetSunrise",
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${userInfo.token}`,
          },
        }
      );
      const updatedSunsetSunrisesData =
        await updatedSunsetSunrisesResponse.json();
        console.log(updatedSunsetSunrisesData);
      setSunsetSunrises(updatedSunsetSunrisesData.data.$values);
      toast.success("Updated sunset sunrise successfully.");
      setAddingRowId(null);
    }
  };

  const handleModifySunsetSunrise = (id) => {
    setAddingRowId(id);
    const city = sunsetSunrises.find((city) => city.id === id);
    setSunsetSunriseData(city);
  };

  const handleCloseForm = () => {
    setAddingRowId(null);
  };
  console.log(sunsetSunrises);
  console.log(sunsetSunriseData);

  return (
    <table className="table table-striped">
      <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">Date</th>
          <th scope="col">Sunrise</th>
          <th scope="col">Sunset</th>
          <th scope="col">City ID</th>
          <th scope="col">Actions</th>
        </tr>
      </thead>
      <tbody>
        {sunsetSunrises.map((sunsetSunrise, index) => (
          <>
            {console.log(sunsetSunrise.id)}
            <tr
              style={{
                backgroundColor: `rgba(255, 255, 255, ${
                  index % 2 === 0 ? 0.1 : 0.3
                })`,
              }}
            >
              <th scope="row">{index + 1}</th>
              <td>{sunsetSunrise.date}</td>
              <td>{sunsetSunrise.sunrise}</td>
              <td>{sunsetSunrise.sunset}</td>
              <td>{sunsetSunrise.cityId}</td>
              <td>
                <PiGearBold
                  className="modify-icon"
                  onClick={() => handleModifySunsetSunrise(sunsetSunrise.id)}
                />
                <MdDeleteForever
                  className="delete-icon"
                  onClick={() => handleDeleteSunsetSunrise(sunsetSunrise.id)}
                />
              </td>
            </tr>
            {addingRowId === sunsetSunrise.id && (
              <tr className="data-to-modify">
                <th scope="row">{index + 1}</th>
                <td>
                  <input
                    type="text"
                    value={sunsetSunriseData.date}
                    placeholder={sunsetSunriseData.date}
                    onChange={(e) =>
                      setSunsetSunriseData({
                        ...sunsetSunriseData,
                        date: e.target.value,
                      })
                    }
                    style={{
                      width: "70%",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={sunsetSunriseData.sunrise}
                    placeholder={sunsetSunriseData.sunrise}
                    onChange={(e) =>
                      setSunsetSunriseData({
                        ...sunsetSunriseData,
                        sunrise: e.target.value,
                      })
                    }
                    style={{
                      width: "70%",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={sunsetSunriseData.sunset}
                    placeholder={sunsetSunriseData.sunset}
                    onChange={(e) =>
                      setSunsetSunriseData({
                        ...sunsetSunriseData,
                        sunset: e.target.value,
                      })
                    }
                    style={{
                      width: "70%",
                    }}
                  />
                </td>
                <td>
                  <input
                    type="text"
                    value={sunsetSunriseData.cityId}
                    placeholder={sunsetSunriseData.cityId}
                    onChange={(e) =>
                      setSunsetSunriseData({
                        ...sunsetSunriseData,
                        cityId: e.target.value,
                      })
                    }
                    style={{
                      width: "25%",
                    }}
                  />
                </td>
                <td>
                  <FaRegSave
                    className="save-icon"
                    onClick={() => handleUpdateSunsetSunrise()}
                  />
                  <RiArrowGoBackLine
                    className="close-icon"
                    onClick={() => handleCloseForm()}
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
export default SunsetSunriseDataTable;
