// src/pages/ProfilePage.jsx
import React, { useEffect, useState } from "react";
import { useParams }  from "react-router-dom";
import { api } from "../api";

export default function ProfilePage() {
  const { id } = useParams();
  const [user, setUser] = useState(null);
  const [participations, setParticipations] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  
  const currentUser = JSON.parse(localStorage.getItem("user"));
  const isOwnProfile = currentUser?.Id == id || currentUser?.id == id;

  useEffect(() => {
    if (!id) {
      setError("ID пользователя не указан");
      setLoading(false);
      return;
    }
    
    setLoading(true);
    setError(null);
    
    // Получаем данные пользователя
    api.get(`/user/${id}`)
      .then(userRes => {
        console.log("User data:", userRes.data);
        setUser(userRes.data);
        // Получаем историю участия
        return api.get(`/participation/user/${id}`);
      })
      .then(partRes => {
        console.log("Participations:", partRes.data);
        setParticipations(partRes.data);
        setLoading(false);
      })
      .catch(err => {
        console.error("Profile error:", err);
        setError(err.response?.data?.message || err.message || "Не удалось загрузить профиль");
        setLoading(false);
      });
  }, [id]);

  if (loading) {
    return (
      <div className="profile-loading">
        <div className="spinner"></div>
        <p>Загрузка профиля...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="profile-error">
        <p className="error-message">{error}</p>
        <button className="btn-secondary" onClick={() => window.history.back()}>Назад</button>
      </div>
    );
  }

  if (!user) {
    return (
      <div className="profile-error">
        <p className="error-message">Пользователь не найден</p>
        <button className="btn-secondary" onClick={() => window.history.back()}>Назад</button>
      </div>
    );
  }

  return (
    <div className="profile-page">
      <div className="profile-header">
        <div className="profile-avatar">
          {user.name?.charAt(0)?.toUpperCase() || "?"}
        </div>
        <div className="profile-info">
          <h1>{user.name || "Без имени"}</h1>
          <p className="profile-role">{user.role || "Участник"}</p>
          {isOwnProfile && <span className="badge">Это вы</span>}
        </div>
      </div>

      <div className="profile-stats">
        <div className="stat-card">
          <h3>Всего баллов</h3>
          <p className="stat-number">{user.total_points ?? 0}</p>
        </div>
        <div className="stat-card">
          <h3>Мероприятий</h3>
          <p className="stat-number">{participations.length}</p>
        </div>
        {user.city && (
          <div className="stat-card">
            <h3>Город</h3>
            <p className="stat-text">{user.city}</p>
          </div>
        )}
        {user.role === "Organizer" && (
          <div className="stat-card">
            <h3>Проведено мероприятий</h3>
            <p className="stat-number">0</p>
          </div>
        )}
      </div>

      <h2 className="section-title">Участие в мероприятиях</h2>
      {participations.length === 0 ? (
        <div className="empty-state">
          <p>Пользователь пока не участвовал ни в одном мероприятии.</p>
        </div>
      ) : (
        <div className="grid-3">
          {participations.map(p => (
            <div key={p.idParticipation} className="event-card">
              <h3>{p.eventTitle || "Мероприятие"}</h3>
              <p className="event-date">
                📅 {p.eventDate ? new Date(p.eventDate).toLocaleDateString() : "Дата неизвестна"}
              </p>
              <p className="event-points">
                🏆 Баллы: <strong>{p.pointsEarned ?? 0}</strong>
              </p>
              <p className="event-status">
                {p.statusId === 2 ? "✅ Подтверждено" : "📝 Зарегистрирован"}
              </p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}