import "./Home.css";

export default function Home() {
  return (
    <div className="home">
      <div className="title">
        <h1
          className="text-center"
          style={{ marginTop: "20px", fontSize: "50px" }}
        >
          Welcome to Solar Watch
        </h1>
      </div>
      <div className="quote">
        <p style={{ marginTop: "60px" }}>
          "Sunrise looks spectacular in the nature; sunrise looks spectacular in
          the photos; sunrise looks spectacular in our dreams; sunrise looks
          spectacular in the paintings, because it really is spectacular!"
        </p>
        <p style={{ marginTop: "20px", textAlign: "right" }}>
          <i>- Mehmet Murat ildan</i>
        </p>
      </div>
    </div>
  );
}
