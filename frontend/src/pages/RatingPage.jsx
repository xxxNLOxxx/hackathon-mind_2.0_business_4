// src/pages/RatingsPage.jsx
import React, { useEffect, useState } from "react";
import { api } from "../api";

function RatingsPage() {
  const [ratings, setRatings] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // TODO: заменить на реальный API
    const mockRatings = [
      { id_user: 1, full_name: "Иван Иванов", total_points: 30, rank: 1 },
      { id_user: 2, full_name: "Мария Петрова", total_points: 45, rank: 2 }
    ];
    setTimeout(() => {
      setRatings(mockRatings);
      setLoading(false);
    }, 500);
  }, []);

  if (loading) {
    return <div className="empty-state">Загрузка рейтинга...</div>;
  }

  return (
    <div className="fade-in">
      <h1>Глобальный рейтинг участников</h1>
      <div className="table-wrapper">
        <table>
          <thead>
            <tr>
              <th> Ранг</th>
              <th> Имя</th>
              <th> Баллы</th>
            </tr>
          </thead>
          <tbody>
            {ratings.map(user => (
              <tr key={user.id_user}>
                <td><strong>#{user.rank}</strong></td>
                <td>{user.full_name}</td>
                <td><span style={{ color: "#10b981", fontWeight: 600 }}>{user.total_points}</span></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default RatingsPage;