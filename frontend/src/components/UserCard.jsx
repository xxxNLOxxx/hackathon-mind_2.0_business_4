// src/components/UserCard.jsx
import React from "react";
import "./UserCard.css";

function UserCard({ user }) {
  // Берем первую букву имени для аватара
  const initials = user.full_name?.split(" ").map(n => n[0]).join("").toUpperCase();

  return (
    <div className="user-card">
      <div className="avatar">{initials}</div>
      <div className="user-info">
        <h3>{user.full_name}</h3>
        <p>Роль: {user.role}</p>
        <p>Город: {user.city}</p>
        <p>Возраст: {user.age}</p>
        <div className="points-bar">
          <div className="points-fill" style={{ width: `${user.total_points || 0}%` }}></div>
        </div>
        <p className="points-text">Баллы: {user.total_points || 0}</p>
      </div>
    </div>
  );
}

export default UserCard;