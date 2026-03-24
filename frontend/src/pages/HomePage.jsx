// src/pages/HomePage.jsx
import React from "react";

function HomePage() {
  return (
    <div className="fade-in">
      <div style={{
        background: "linear-gradient(135deg, #3b82f6, #2563eb)",
        borderRadius: "24px",
        padding: "48px",
        color: "white",
        textAlign: "center",
        marginBottom: "40px"
      }}>
        <h1 style={{ color: "white", marginBottom: "16px", fontSize: "2rem" }}>Добро пожаловать!</h1>
        <p style={{ fontSize: "1rem", opacity: 0.9 }}>
          Платформа рейтинга активности молодежного парламента
        </p>
      </div>
      
      <div className="grid-3">
        <div style={{ background: "white", borderRadius: "16px", padding: "28px", textAlign: "center", border: "1px solid #e2e8f0" }}>
          <h3 style={{ marginBottom: "10px", color: "#0f172a" }}>Мероприятия</h3>
          <p style={{ color: "#64748b", fontSize: "0.9rem" }}>Участвуй в событиях и получай баллы</p>
        </div>
        <div style={{ background: "white", borderRadius: "16px", padding: "28px", textAlign: "center", border: "1px solid #e2e8f0" }}>
          <h3 style={{ marginBottom: "10px", color: "#0f172a" }}>Рейтинг</h3>
          <p style={{ color: "#64748b", fontSize: "0.9rem" }}>Соревнуйся и попади в кадровый резерв</p>
        </div>
        <div style={{ background: "white", borderRadius: "16px", padding: "28px", textAlign: "center", border: "1px solid #e2e8f0" }}>
          <h3 style={{ marginBottom: "10px", color: "#0f172a" }}>Сообщество</h3>
          <p style={{ color: "#64748b", fontSize: "0.9rem" }}>Знакомься с активными участниками</p>
        </div>
      </div>
    </div>
  );
}

export default HomePage;